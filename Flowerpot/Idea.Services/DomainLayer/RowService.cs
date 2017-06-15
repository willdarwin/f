using System;
using System.Collections.Generic;
using System.Text;
using IdeaDomain.DomainLayer.Entities;
using IdeaDomain.DomainLayer.RepositoryInterfaces;
using IdeaDomain.ServiceLayer;

namespace IdeaDomain.DomainLayer
{
    public class RowService : IRowService
    {

        public IRowRepository RowRepository { get; set; }

        public RowService(IRowRepository rowRepository)
        {
            RowRepository = rowRepository;
        }

        public Row GetRowById(int rowId, int userId)
        {
            var row = new Row();
            row = RowRepository.GetRowById(rowId,userId);
            return row;
        }

        public bool AddRow(Row row)
        {
            var result = false;
            result = RowRepository.AddRow(row);
            return result;
        }

        public bool UpdateRow(Row row)
        {
            var result = false;
            result = RowRepository.UpdateRow(row);
            return result;
        }

        public bool DeleteRow(int rowId, int userId)
        {
            var result = false;
            result = RowRepository.DeleteRow(rowId, userId);
            return result;
        }

    }
}
