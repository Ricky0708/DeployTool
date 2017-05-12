using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Security;

namespace IISOperator
{
    public interface IOperatorProvider
    {
        void StopWebSite(string webSite);
        void StopAppPool(string appPoolName);
        void StartAppPool(string appPoolName);
        void StartWebSite(string webSite);
    }
    public class OperatorProvider : IOperatorProvider
    {
        public void StopWebSite(string webSite)
        {
            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddScript($@"
                        try{{
                            Import-Module WebAdministration
                            Stop-WebSite '{webSite}'
                        }}
                        catch [Exception]{{
                            $_.Exception.Message;
                        }}"
                    );
                var invokeResult = ps.Invoke();
                if (ps.HadErrors)
                {
                    PSErrorProcess(ps);
                }
                if (invokeResult.Count() > 0)
                {
                    ResultErrorProcess(invokeResult);
                }
            }
        }

        public void StopAppPool(string appPoolName)
        {

            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddScript($@"    
                        try {{
                            Import-Module WebAdministration
                            if((Get-WebAppPoolState '{appPoolName}').Value -ne 'Stopped'){{
                                Stop-WebAppPool '{appPoolName}'
                            }}
                        }}
                        catch [Exception]{{
                            $_.Exception.Message;
                        }}"
                    );
                var invokeResult = ps.Invoke();
                if (ps.HadErrors)
                {
                    PSErrorProcess(ps);
                }
                if (invokeResult.Count() > 0)
                {
                    ResultErrorProcess(invokeResult);
                }
            }
        }

        public void StartAppPool(string appPoolName)
        {

            using (PowerShell ps = PowerShell.Create())
            {

                ps.AddScript($@"    
                        try {{
                            Import-Module WebAdministration
                            if((Get-WebAppPoolState '{appPoolName}').Value -ne 'Started'){{
                                Start-WebAppPool '{appPoolName}'
                            }}
                        }}
                        catch [Exception]{{
                            $_.Exception.Message;
                        }}"
                    );
                var invokeResult = ps.Invoke();
                if (ps.HadErrors)
                {
                    PSErrorProcess(ps);
                }
                if (invokeResult.Count() > 0)
                {
                    ResultErrorProcess(invokeResult);
                }
            }
        }

        public void StartWebSite(string webSite)
        {

            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddScript($@"
                        try{{
                            Import-Module WebAdministration
                            Start-WebSite '{webSite}'
                        }}
                        catch [Exception]{{
                            $_.Exception.Message;
                        }}"
                    );
                var invokeResult = ps.Invoke();
                if (ps.HadErrors)
                {
                    PSErrorProcess(ps);
                }
                if (invokeResult.Count() > 0)
                {
                    ResultErrorProcess(invokeResult);
                }
            }
        }

        private void PSErrorProcess(PowerShell ps)
        {
            List<string> errorList = new List<string>();
            foreach (ErrorRecord _ErrorRecord in ps.Streams.Error)
            {
                errorList.Add(_ErrorRecord.ToString());
                Console.WriteLine(_ErrorRecord.ToString());
            }
            var errorMsg = string.Join(",", errorList.ToArray());
            if (string.IsNullOrEmpty(errorMsg))
                return;

            throw new Exception(errorMsg);
        }

        private void ResultErrorProcess(Collection<PSObject> psObjects)
        {
            List<string> errorList = new List<string>();
            foreach (var item in psObjects)
            {
                errorList.Add(item.ToString());
                Console.WriteLine(item.ToString());
            }
            var errorMsg = string.Join(",", errorList.ToArray());
            if (string.IsNullOrEmpty(errorMsg))
                return;

            throw new Exception(errorMsg);
        }

    }
}
