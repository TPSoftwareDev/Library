using System.Data;
using System.Text;

namespace Teleperformance.Data
{
	public static class CommandHelper
	{
		public static string ToNameValueString(this IDataParameterCollection parameters)
		{
			StringBuilder stringBuilder = new StringBuilder();

			foreach (IDataParameter parameter in parameters)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(", ");
				}

				stringBuilder.AppendFormat("{0} = {1}", parameter.ParameterName, parameter.Value);
			}

			return stringBuilder.ToString();
		}
	}
}