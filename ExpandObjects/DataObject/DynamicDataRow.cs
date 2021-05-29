using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qrame.CoreFX.ExpandObjects.DataObject
{
    /*
    DataTable table = new DataTable();
    table.Columns.Add("FirstName", typeof(string));
    table.Columns.Add("LastName", typeof(string));
    table.Columns.Add("DateOfBirth", typeof(DateTime));
 
    dynamic row = table.NewRow().AsDynamic();
    row.FirstName = "John";
    row.LastName = "Doe";
    row.DateOfBirth = new DateTime(1981, 9, 12);
    table.Rows.Add(row.DataRow);
 
    // Add more rows...
    // ...
 
    var bornInThe20thCentury = from r in table.AsEnumerable()
                                let dr = r.AsDynamic()
                                where dr.DateOfBirth.Year > 1900
                                && dr.DateOfBirth.Year <= 2000
                                select new { dr.LastName, dr.FirstName };
 
    foreach (var item in bornInThe20thCentury)
    {
        Console.WriteLine("{0} {1}", item.FirstName, item.LastName);
    }
    */
    public class DynamicDataRow : DynamicObject
    {
        private DataRow _dataRow;

        public DynamicDataRow(DataRow dataRow)
        {
            if (dataRow == null)
                throw new ArgumentNullException("dataRow");
            this._dataRow = dataRow;
        }

        public DataRow DataRow
        {
            get { return _dataRow; }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            if (_dataRow.Table.Columns.Contains(binder.Name))
            {
                result = _dataRow[binder.Name];
                return true;
            }
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_dataRow.Table.Columns.Contains(binder.Name))
            {
                _dataRow[binder.Name] = value;
                return true;
            }
            return false;
        }
    }
}
