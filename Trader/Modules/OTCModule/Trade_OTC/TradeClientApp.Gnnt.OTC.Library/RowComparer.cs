using System;
using System.Collections;
using System.Windows.Forms;
namespace TradeClientApp.Gnnt.OTC.Library
{
	public class RowComparer : IComparer
	{
		private int sortOrderModifier = 1;
		private int index;
		private string columnName = string.Empty;
		public RowComparer(SortOrder sortOrder, int _index, string _columnName)
		{
			this.index = _index;
			this.columnName = _columnName;
			if (sortOrder == SortOrder.Descending)
			{
				this.sortOrderModifier = -1;
				return;
			}
			if (sortOrder == SortOrder.Ascending)
			{
				this.sortOrderModifier = 1;
			}
		}
		public int Compare(object x, object y)
		{
			DataGridViewRow dataGridViewRow = (DataGridViewRow)x;
			DataGridViewRow dataGridViewRow2 = (DataGridViewRow)y;
			int num = string.Compare(dataGridViewRow.Cells[this.columnName].Value.ToString(), dataGridViewRow2.Cells[this.columnName].Value.ToString());
			if (num == 0)
			{
				num = string.Compare(dataGridViewRow.Cells[this.index].Value.ToString(), dataGridViewRow2.Cells[this.index].Value.ToString());
			}
			return num * this.sortOrderModifier;
		}
	}
}
