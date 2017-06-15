using System;
using System.Collections.Generic;
using System.Text;
using IdeaDomain.DomainLayer.Entities;

namespace IdeaDomain.ServiceLayer
{
    public interface IRowService
    {
        Row GetRowById(int rowId, int userId);

        bool AddRow(Row row);

        bool UpdateRow(Row row);

        bool DeleteRow(int rowId, int userId);
    }
}
