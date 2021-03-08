using Forum_API.Common;
using Forum_API.Entities;
using Forum_API.Helpers;
using Forum_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Unity;

namespace Forum_API.Services
{
    public class SystemConfigService : GeneralService<SystemConfigEntity>, ISystemConfigService
    {
        [Dependency]
        public ILoggerManager Logger { get; set; }
        private IDictionary<string,string> Configs { get; set; }

        public async Task<T> GetConfig<T>(ConfigCode code)
        {
            return await this.GetConfig<T>(code.ToString());
        }
        public async Task<T> GetConfig<T>(string code)
        {
            if (this.Configs == null)
            {
                var data = await this.GetAll();
                this.Configs = data.ToDictionary(x => x.Code, x => x.Value);
            }
            if (this.Configs.ContainsKey(code.ToString()))
            {
                return (T)Convert.ChangeType(this.Configs[code.ToString()], typeof(T));
            }
            return default(T);
        }
        public async Task<bool> Hooking(ConfigCode code, IDictionary<string,string> param)
        {
            try
            {
                var hooking = await this.GetConfig<string>(code);
                if (hooking != null && hooking != string.Empty)
                {
                    using (var http = new HttpClient())
                    {
                        var data = new FormUrlEncodedContent(param);
                        http.Timeout = TimeSpan.FromSeconds(1);
                        var w = new Stopwatch();
                        w.Start();
                        var resp = await http.PostAsync(hooking, data);
                        this.Logger.Info($"Sent hooking request with {resp.StatusCode} status code in {w.ElapsedMilliseconds}ms");
                        return resp.IsSuccessStatusCode;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                this.Logger.Info($"HOOKING ERROR: {ex.Message}");
                return false;
            }
            
        }

        public async Task<long> GetDefaultRole()
        {
            var roleId = await this.GetConfig<long>(ConfigCode.DEFAULT_ROLE);
            return roleId;
        }
    }
}
