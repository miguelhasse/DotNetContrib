using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace Hasseware.Resources
{
    public sealed class SqlResourceProvider : ResourceProvider
    {
        private DbProviderFactory factory;

        private string connectionString;

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (string.IsNullOrEmpty(name))
            {
                name = "SqlResourceProvider";
            }
            base.Initialize(name, config);

            string connectionStringName = config["connectionStringName"];
            if (String.IsNullOrEmpty(connectionStringName))
            {
                throw new ProviderException(Properties.Resources.Connection_name_not_specified);
            }
            var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (connectionStringSettings == null)
            {
                throw new ProviderException(String.Format(CultureInfo.CurrentUICulture,
                    Properties.Resources.Connection_string_not_found, connectionStringName));
            }
            factory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
            connectionString = connectionStringSettings.ConnectionString;
            config.Remove("connectionStringName");

            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!String.IsNullOrEmpty(key))
                {
                    throw new ProviderException(String.Format(CultureInfo.CurrentUICulture,
                        Properties.Resources.Provider_unrecognized_attribute, key));
                }
            }
        }

        public override IDictionary<string, object> Get(string resourceSet, CultureInfo culture)
        {
            using (var conn = PrepareConnection())
            {
                const string stmt = "SELECT ResourceKey, Value, MimeType FROM Resources WHERE (ResourceSet=@p1) AND (LocaleId=@p2)";
                using (var cmd = PrepareCommand(conn, stmt, resourceSet, culture.LCID))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        Dictionary<string, object> resultSet = new Dictionary<string, object>();

                        while (reader.Read())
                        {
                            string mimeType = reader.IsDBNull(2) ? null : reader.GetString(2);
                            object value = reader.IsDBNull(1) ? null : Converter.ConvertFromStore(reader.GetValue(1), mimeType);
                            resultSet.Add(reader.GetString(0), value);
                        }
                        return resultSet;
                    }
                }
            }
        }

        public override object Get(string key, string resourceSet, CultureInfo culture)
        {
            using (var conn = PrepareConnection())
            {
                const string stmt = "SELECT Value, MimeType FROM Resources WHERE (ResourceKey=@p1) AND (ResourceSet=@p2) AND (LocaleId=@p3)";
                using (var cmd = PrepareCommand(conn, stmt, key, resourceSet, culture.LCID))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string mimeType = reader.IsDBNull(1) ? null : reader.GetString(1);
                            return reader.IsDBNull(0) ? null : Converter.ConvertFromStore(reader.GetValue(0), mimeType);
                        }
                        return null;
                    }
                }
            }
        }

        public override void Save(IDictionary<string, object> resources, string resourceSet, CultureInfo culture)
        {
            using (var conn = PrepareConnection())
            {
                using (var trans = conn.BeginTransaction())
                {
                    using (var updcmd = PrepareUpdateCommand(trans, resourceSet, culture))
                    {
                        using (var inscmd = PrepareInsertCommand(trans, resourceSet, culture))
                        {
                            foreach (var res in resources)
                            {
                                string mimeType;
                                object value = Converter.ConvertToStore(res.Value, out mimeType);

                                updcmd.Parameters["@p1"].Value = res.Key;
                                updcmd.Parameters["@p2"].Value = value;
                                updcmd.Parameters["@p3"].Value = mimeType;

                                if (updcmd.ExecuteNonQuery() == 0)
                                {
                                    inscmd.Parameters["@p1"].Value = res.Key;
                                    inscmd.Parameters["@p2"].Value = value;
                                    updcmd.Parameters["@p3"].Value = mimeType;

                                    inscmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    trans.Commit();
                }
            }
        }

        public override void Save(string key, object value, string resourceSet, CultureInfo culture)
        {
            Save(new Dictionary<string, object> { { key, value } }, resourceSet, culture);
        }

        public override void Delete(string key, string resourceSet, CultureInfo culture)
        {
            using (var conn = PrepareConnection())
            {
                const string stmt = "DELETE Resources WHERE (@p1 IS NULL OR ResourceKey=@p1) AND (ResourceSet=@p2) AND (LocaleId=@p3)";
                using (var cmd = PrepareCommand(conn, stmt, key, resourceSet, culture.LCID))
                    cmd.ExecuteNonQuery();
            }
        }

        private DbCommand PrepareInsertCommand(DbTransaction transaction, string resourceSet, CultureInfo culture)
        {
            const string stmt = "INSERT INTO Resources (ResourceKey, Value, MimeType, ResourceSet, LocaleId) VALUES (@p1, @p2, @p3, @p4, @p5)";
            var cmd = PrepareCommand(transaction.Connection, stmt, null, null, null, resourceSet, culture.LCID);
            cmd.Transaction = transaction;
            return cmd;
        }

        private DbCommand PrepareUpdateCommand(DbTransaction transaction, string resourceSet, CultureInfo culture)
        {
            const string stmt = "UPDATE Resources SET Value=@p2, MimeType=@p3 WHERE (ResourceKey=@p1) AND (ResourceSet=@p4) AND (LocaleId=@p5)";
            var cmd = PrepareCommand(transaction.Connection, stmt, null, null, null, resourceSet, culture.LCID);
            cmd.Transaction = transaction;
            return cmd;
        }

        private DbConnection PrepareConnection()
        {
            var con = factory.CreateConnection();
            con.ConnectionString = connectionString;
            con.Open();
            return con;
        }

        private DbCommand PrepareCommand(DbConnection connection, string statement, params object[] args)
        {
            var cmd = factory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = statement;
            cmd.Connection = connection;

            for (int n = 0; n < args.Length; n++)
            {
                var param = cmd.CreateParameter();
                cmd.Parameters.Add(param);

                param.ParameterName = String.Format("@p{0}", n + 1);
                param.Value = args[n] ?? DBNull.Value;
            }
            return cmd;
        }
    }
}
