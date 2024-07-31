using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.Abstraction;

public interface IDatabaseService
{
	public IDbConnection CreateConnection();
}
