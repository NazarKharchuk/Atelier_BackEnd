﻿namespace Atelier.BLL.DTO
{
    public class FilteredClientListRequestDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string Sort { get; set; }
    }
}
