using Project01.Helpers;
using Project01.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Project01.Helpers.DbException;

namespace Project01.Services
{
    public class SqlServerDbService : IDbService
    {
        public IEnumerable<Models.Task> GetTasks(string member)
        { 
            var _assign = new List<Models.Task>();
            var _creator = new List<Models.Task>();

            var members = new List<string> { "assigned", "creator" };

            using (var con = new SqlConnection(GlobalDatabase.connectionString))
            {
                using (var com = new SqlCommand())
                {
                    if (members.Contains(member))
                    {
                        switch (member.ToLower())
                        {
                            case "assigned":
                                com.CommandText = @"select t.IdTask, t.Name, t.Description, t.Deadline, tp.name 
                                                    from task t, tasktype tp, teamMember tm 
                                                    wheret.idtasktype = tp.idtasktype and t.idcreator = tm.idteammember; ";
                                break;
                            case "creator":
                                com.CommandText = @"select t.IdTask, t.Name, t.Description, t.Deadline, tp.name 
                                                    from task t, tasktype tp, teamMember tm 
                                                    where t.idtasktype = tp.idtasktype and t.IdAssignedTo = tm.idteammember;";
                                break;
                            default:
                                return null;
                        }

                        con.Open();
                        using var dr = com.ExecuteReader();

                        while (dr.Read())
                        {
                            switch (member.ToLower())
                            {
                                case "assigned":
                                    var asg = new Models.Task
                                    {
                                        IdTask = int.Parse(dr["IdTask"].ToString()),
                                        Name = dr["Name"].ToString(),
                                        Description = dr["Description"].ToString(),
                                        Deadline = DateTime.Parse(dr["Deadline"].ToString()),
                                        TaskType = dr["name"].ToString()
                                    };
                                    _assign.Add(asg);
                                    break;
                                case "creator":
                                    var crt = new Models.Task
                                    {
                                        IdTask = int.Parse(dr["IdTask"].ToString()),
                                        Name = dr["Name"].ToString(),
                                        Description = dr["Description"].ToString(),
                                        Deadline = DateTime.Parse(dr["Deadline"].ToString()),
                                        TaskType = dr["name"].ToString()
                                    };
                                    _creator.Add(crt);
                                    break;
                            }
                        }

                        if (member.ToLower() == "assigned")
                            return _assign;
                        else if (member.ToLower() == "creator")
                            return _creator;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
