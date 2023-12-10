using Atelier.BLL.DTO;
using Atelier.BLL.Infrastructure;
using Atelier.BLL.Interfaces;
using Atelier.DAL.Entities;
using Atelier.DAL.Enums;
using Atelier.DAL.Interfaces;
using AutoMapper;
using System.Text;

namespace Atelier.BLL.Services
{
    public class OrderService : IOrderService<OrderDTO>
    {
        IUnitOfWork DataBase { get; set; }
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork uow, IMapper mapper)
        {
            DataBase = uow;
            _mapper = mapper;
        }

        public List<OrderDTO> GetAll()
        {
            return _mapper.Map<List<OrderDTO>>(DataBase.Orders.GetAll());
        }

        public async Task<OrderDTO> Get(int id)
        {
            var item = await DataBase.Orders.Get(id);
            if (item == null)
                throw new Exception("Замовлення не знайдено");

            return _mapper.Map<OrderDTO>(item);
        }

        public async Task CreateOrderWithMaterial(OrderDTO order, List<OrderMaterialDTO> om_list)
        {
            using (var transaction = await DataBase.BeginTransactionAsync())
            {
                try
                {
                    await Create(order);
                    var added_order = DataBase.Orders.Find(x => x.ReceivingDate == order.ReceivingDate && x.IssueDate == order.IssueDate && x.WorkTypeId == order.WorkTypeId &&
                     x.ClientId == order.ClientId && x.EmployeeId == order.EmployeeId).FirstOrDefault();
                    foreach (var om in om_list)
                    {
                        om.OrderId = added_order.OrderId;
                        await AddMaterialWithoutTransaction(om.OrderId, om);
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception($"Помилка на етапі створення замовлення з матеріалами({e.Message})");
                }
            }
        }

        public async Task Create(OrderDTO item)
        {
            if (item.ReceivingDate > DateTime.Now)
                throw new ValidationException("Не коректна початкова дата", "");
            if (item.IssueDate < item.ReceivingDate)
                throw new ValidationException("Не коректна кінцева дата", "");
            if (await DataBase.Clients.Get(item.ClientId) == null)
                throw new ValidationException("Неіснуючий клієнт", "");
            if (item.EmployeeId != null && await DataBase.Clients.Get((int)item.EmployeeId) == null)
                throw new ValidationException("Неіснуючий працівник", "");
            if (await DataBase.WorksTypes.Get(item.WorkTypeId) == null)
                throw new ValidationException("Неіснуюча послуга", "");

            await DataBase.Orders.Create(_mapper.Map<Order>(item));
            await DataBase.SaveAsync();
        }

        public async Task Update(OrderDTO item)
        {
            if (item.ReceivingDate > DateTime.Now)
                throw new ValidationException("Не коректна початкова дата", "");
            if (item.IssueDate < item.ReceivingDate)
                throw new ValidationException("Не коректна кінцева дата", "");
            if (await DataBase.Clients.Get(item.ClientId) == null)
                throw new ValidationException("Неіснуючий клієнт", "");
            if (item.EmployeeId != null && await DataBase.Clients.Get((int)item.EmployeeId) == null)
                throw new ValidationException("Неіснуючий працівник", "");
            if (await DataBase.WorksTypes.Get(item.WorkTypeId) == null)
                throw new ValidationException("Неіснуюча послуга", "");

            Order order = await DataBase.Orders.Get(item.OrderId);
            if (order == null)
                throw new Exception("Замовлення не знайдено");
            if ((int)order.Status == (int)Status.Completed)
                throw new Exception("Завершене замовлення змінювати не можна");
            if ((int)order.Status > (int)item.Status)
                throw new Exception("Статус не можна понижувати до попередніх етапів");
            if ((int)item.Status == (int)Status.Completed)
            {
                using (var transaction = await DataBase.BeginTransactionAsync())
                {
                    try
                    {
                        var res = await GetAllMaterials(item.OrderId);
                        foreach (var i in res)
                        {
                            await UpdateReserve(i.MaterialId, -i.Count);
                            await UpdateQuantity(i.MaterialId, -i.Count);
                        }
                        await DataBase.SaveAsync();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception($"Помилка не етапі скасування резерву матеріалів виконаного замовлення:({e.Message})");
                    }
                }
            }
            order.ReceivingDate = item.ReceivingDate;
            order.IssueDate = item.IssueDate;
            order.Status = item.Status;
            order.WorkTypeId = item.WorkTypeId;
            order.EmployeeId = item.EmployeeId;
            order.ClientId = item.ClientId;
            DataBase.Orders.Update(order);
            await DataBase.SaveAsync();
        }

        public async Task UpdateStatus(int order_id, Status newStatus)
        {
            Order order = await DataBase.Orders.Get(order_id);
            if (order == null)
                throw new Exception("Замовлення не знайдено");
            if ((int)order.Status == (int)Status.Completed)
                throw new Exception("Завершене замовлення змінювати не можна");
            if ((int)order.Status > (int)newStatus)
                throw new Exception("Статус не можна понижувати до попередніх етапів");
            if ((int)newStatus == (int)Status.Completed)
            {
                using (var transaction = await DataBase.BeginTransactionAsync())
                {
                    try
                    {
                        var res = await GetAllMaterials(order_id);
                        foreach (var i in res)
                        {
                            await UpdateReserve(i.MaterialId, -i.Count);
                            await UpdateQuantity(i.MaterialId, -i.Count);
                        }
                        await DataBase.SaveAsync();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception($"Помилка не етапі скасування резерву матеріалів виконаного замовлення:({e.Message})");
                    }
                }
            }
            order.Status = newStatus;
            DataBase.Orders.Update(order);
            await DataBase.SaveAsync();
        }

        public async Task<List<OrderMaterialDTO>> GetAllMaterials(int order_id)
        {
            var o = await DataBase.Orders.Get(order_id);
            if (o == null)
                throw new ValidationException("Замовлення не знайдено", "");

            return _mapper.Map<List<OrderMaterialDTO>>(DataBase.Orders.GetAllMaterials(order_id));
        }

        public async Task AddMaterial(int OrderId, OrderMaterialDTO order_material)
        {
            using (var transaction = await DataBase.BeginTransactionAsync())
            {
                try
                {
                    await AddMaterialWithoutTransaction(OrderId, order_material);
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception($"Помилка на етапі додавання матеріалу для замовлення({e.Message})");
                }
            }
        }

        private async Task AddMaterialWithoutTransaction(int OrderId, OrderMaterialDTO order_material)
        {
            var o = await DataBase.Orders.Get(OrderId);
            if (o == null)
                throw new ValidationException("Замовлення не знайдено", "");
            var m = await DataBase.Materials.Get(order_material.MaterialId);
            if (m == null)
                throw new ValidationException("Матеріал не знайдено", "");
            if (o.OrderId != order_material.OrderId)
                throw new Exception("Не коректне Order_Material");
            if (order_material.Count <= 0)
                throw new ValidationException("Не коректна кількість матеріалу", "");
            DataBase.Orders.AddMaterial(_mapper.Map<Order>(o), _mapper.Map<OrderMaterial>(order_material));
            if (order_material.Count > (m.Quantity - m.Reserve))
                throw new ValidationException("Error Material Count", "");
            await UpdateReserve(order_material.MaterialId, order_material.Count);
            await DataBase.SaveAsync();
        }

        private async Task UpdateReserve(int id, decimal value)
        {
            var item = await DataBase.Materials.Get(id);
            if (item == null)
                throw new Exception("Матеріал не знайдено");

            if (item.Reserve + value < 0 || item.Reserve + value > item.Quantity)
                throw new ValidationException("Не коректна величина резерву матеріалу", "");

            item.Reserve += value;
            DataBase.Materials.Update(item);
            await DataBase.SaveAsync();
        }

        private async Task UpdateQuantity(int id, decimal value)
        {
            var item = await DataBase.Materials.Get(id);
            if (item == null)
                throw new Exception("Матеріал не знайдено");

            if (item.Quantity + value < 0)
                throw new ValidationException("Не коректна кількість матеріалу", "");

            item.Quantity += value;
            DataBase.Materials.Update(item);
            await DataBase.SaveAsync();
        }

        public async Task DeleteMaterial(int order_id, int material_id)
        {
            using (var transaction = await DataBase.BeginTransactionAsync())
            {
                try
                {
                    var o = await DataBase.Orders.Get(order_id);
                    if (o == null)
                        throw new ValidationException("Замовлення не знайдено", "");
                    var m = await DataBase.Materials.Get(material_id);
                    if (m == null)
                        throw new ValidationException("Матеріал не знайдено", "");
                    decimal delta = DataBase.Orders.GetMaterialCount(order_id, material_id);
                    if (delta == -1)
                        throw new ValidationException("Не коректне Order_Material", "");
                    delta *= (-1);
                    await UpdateReserve(material_id, delta);
                    DataBase.Orders.DeleteMaterial(order_id, material_id);
                    await DataBase.SaveAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception($"Помилка на етапі видалення матеріалу({e.Message})");
                }
            }
        }

        public async Task UpdateMaterial(int order_id, OrderMaterialDTO material)
        {
            using (var transaction = await DataBase.BeginTransactionAsync())
            {
                try
                {
                    var o = await DataBase.Orders.Get(order_id);
                    if (o == null)
                        throw new ValidationException("Замовлення не знайдено", "");
                    var m = await DataBase.Materials.Get(material.MaterialId);
                    if (m == null)
                        throw new ValidationException("Матеріал не знайдено", "");
                    if (o.OrderId != material.OrderId)
                        throw new Exception("Не коректне Order_Material");
                    if (material.Count <= 0)
                        throw new ValidationException("Не коректна кількість потрібного матеріалу", "");
                    decimal delta = DataBase.Orders.GetMaterialCount(order_id, material.MaterialId);
                    if (delta == -1)
                        throw new ValidationException("Такий матеріал для замовлення не знайдено", "");
                    delta = material.Count - delta;
                    await UpdateReserve(material.MaterialId, delta);
                    DataBase.Orders.UpdateMaterial(order_id, _mapper.Map<OrderMaterial>(material));
                    await DataBase.SaveAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception($"Помилка на етапі оновлення матеріалу({e.Message})");
                }
            }
        }

        public Tuple<List<OrderResponse>, int> GetOrders(FilteredOrderListRequestDTO filter)
        {
            IEnumerable<OrderResponse> orders = DataBase.Orders.GetOrdersWithDetails();
            if (filter.StartDate != null) orders = orders.Where(x => x.ReceivingDate >= DateTime.Parse(filter.StartDate));
            if (filter.EndDate != null) orders = orders.Where(x => x.ReceivingDate <= DateTime.Parse(filter.EndDate));
            if (filter.Status != null) orders = orders.Where(x => (int)x.Status == filter.Status);
            if (filter.EmployeeFirstName != null) orders = orders.Where(x => x.EmployeeFirstName == filter.EmployeeFirstName);
            if (filter.WorksTypeName != null) orders = orders.Where(x => x.WorkTypeName == filter.WorksTypeName);
            if (filter.Sort != null)
            {
                if (filter.Sort.ToLower() == "desc") orders = orders.OrderByDescending(x => x.ReceivingDate);
                else
                {
                    if (filter.Sort.ToLower() == "asc") orders = orders.OrderBy(x => x.ReceivingDate);
                }
            }
            int count = orders.Count();
            if (filter.PageNumber == null) filter.PageNumber = 1;
            if (filter.PageSize == null) { filter.PageSize = 10; };
            if (filter.PageSize > 20) { filter.PageSize = 20; };
            orders = orders.Skip((int)((filter.PageNumber - 1) * filter.PageSize)).Take((int)filter.PageSize).ToList();
            return Tuple.Create(_mapper.Map<List<OrderResponse>>(orders), count);
        }

        public byte[] ExportOrders()
        {
            IEnumerable<OrderResponse> orders = DataBase.Orders.GetOrdersWithDetails();
            var builder = new StringBuilder();
            builder.AppendLine("OrderId;ReceivingDate;IssueDate;Status;WorkTypeId;WorkTypeName;WorkTypeCost;EmployeeId;EmployeeFirstName;EmployeeLastName;ClientId;" +
                "ClientFirstName;ClientLastName;Price");
            foreach (var item in orders)
            {
                builder.AppendLine($"{item.OrderId};{item.ReceivingDate};{item.IssueDate};{item.Status};{item.WorkTypeId};{item.WorkTypeName};" +
                    $"{item.WorkTypeCost};{item.EmployeeId};{item.EmployeeFirstName};{item.EmployeeLastName};{item.ClientId};" +
                    $"{item.ClientFirstName};{item.ClientLastName};{item.Price}");
            }
            return Encoding.UTF8.GetBytes(builder.ToString());
        }

        public Tuple<int, int> GetMaxMinYear()
        {
            IEnumerable<Order> orders = DataBase.Orders.GetAll();
            int min = 3000;
            int max = 0;
            foreach (var order in orders)
            {
                if (min > order.ReceivingDate.Year)
                {
                    min = order.ReceivingDate.Year;
                }
                if (max < order.ReceivingDate.Year)
                {
                    max = order.ReceivingDate.Year;
                }
            }

            return new Tuple<int, int>(max, min);
        }

        public List<MonthStatisticResponseDTO> GetYearStatistic(int year)
        {
            List<MonthStatisticResponseDTO> statistic = new List<MonthStatisticResponseDTO>();

            statistic.Add(new MonthStatisticResponseDTO() { MonthName = "Січень", NewOrdersCount = 0, InProcessOrdersCount = 0, CompletedOrdersCount = 0 });
            statistic.Add(new MonthStatisticResponseDTO() { MonthName = "Лютий", NewOrdersCount = 0, InProcessOrdersCount = 0, CompletedOrdersCount = 0 });
            statistic.Add(new MonthStatisticResponseDTO() { MonthName = "Березень", NewOrdersCount = 0, InProcessOrdersCount = 0, CompletedOrdersCount = 0 });
            statistic.Add(new MonthStatisticResponseDTO() { MonthName = "Квітень", NewOrdersCount = 0, InProcessOrdersCount = 0, CompletedOrdersCount = 0 });
            statistic.Add(new MonthStatisticResponseDTO() { MonthName = "Травень", NewOrdersCount = 0, InProcessOrdersCount = 0, CompletedOrdersCount = 0 });
            statistic.Add(new MonthStatisticResponseDTO() { MonthName = "Червень", NewOrdersCount = 0, InProcessOrdersCount = 0, CompletedOrdersCount = 0 });
            statistic.Add(new MonthStatisticResponseDTO() { MonthName = "Липень", NewOrdersCount = 0, InProcessOrdersCount = 0, CompletedOrdersCount = 0 });
            statistic.Add(new MonthStatisticResponseDTO() { MonthName = "Серпень", NewOrdersCount = 0, InProcessOrdersCount = 0, CompletedOrdersCount = 0 });
            statistic.Add(new MonthStatisticResponseDTO() { MonthName = "Вересень", NewOrdersCount = 0, InProcessOrdersCount = 0, CompletedOrdersCount = 0 });
            statistic.Add(new MonthStatisticResponseDTO() { MonthName = "Жовтень", NewOrdersCount = 0, InProcessOrdersCount = 0, CompletedOrdersCount = 0 });
            statistic.Add(new MonthStatisticResponseDTO() { MonthName = "Листопад", NewOrdersCount = 0, InProcessOrdersCount = 0, CompletedOrdersCount = 0 });
            statistic.Add(new MonthStatisticResponseDTO() { MonthName = "Грудень", NewOrdersCount = 0, InProcessOrdersCount = 0, CompletedOrdersCount = 0 });

            IEnumerable<Order> orders = DataBase.Orders.GetAll();
            foreach (var order in orders)
            {
                if (order.ReceivingDate.Year == year)
                {
                    switch ((int)order.Status)
                    {
                        case 0:
                            statistic[order.ReceivingDate.Month - 1].NewOrdersCount++;
                            break;
                        case 1:
                            statistic[order.ReceivingDate.Month - 1].InProcessOrdersCount++;
                            break;
                        case 2:
                            statistic[order.ReceivingDate.Month - 1].CompletedOrdersCount++;
                            break;
                    }
                }
            }

            return statistic;
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
