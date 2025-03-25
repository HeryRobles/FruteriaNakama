﻿namespace DevilFruits.DTO.Responses
{
    public class PaginacionResponse<T>
    {
        public List<T> Data { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
