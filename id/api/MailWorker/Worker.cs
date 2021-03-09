//using Cambopay_API.Entities;
//using log4net;
//using log4net.Config;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Options;
//using System;
//using System.IO;
//using System.Linq;
//using System.Net.Mail;
//using System.Reflection;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Xml;

//namespace MailWorker
//{
//    public enum MailStatus
//    {
//        Pendding = 0,
//        Complete
//    }
//    public class Worker : BackgroundService
//    {
//        private readonly ILog logger = LogManager.GetLogger(typeof(Worker));
//        private readonly AppSettings AppSettings;
//        public Worker(IOptions<AppSettings> options)
//        {
//            this.AppSettings = options.Value;
//            XmlDocument log4netConfig = new XmlDocument();

//            using (var fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory +"log4net.config"))
//            {
//                log4netConfig.Load(fs);

//                var repo = LogManager.CreateRepository(
//                        Assembly.GetEntryAssembly(),
//                        typeof(log4net.Repository.Hierarchy.Hierarchy));

//                XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
//            }
//        }
//        private async Task Execute()
//        {
//            using(var db = new DatabaseContext())
//            {
//                var success = 0;
//                try
//                {
//                    logger.Info("Begin scan email");
//                    var emails = await db.MailSending
//                        .Where(x => x.IsDeleted != '1'  && x.Status == (int)MailStatus.Pendding)
//                        .Include(x => x.User)
//                        .ToListAsync();
//                    if (emails.Any())
//                    {
//                        logger.Info($"Have {emails.Count} mail pending send");
//                        foreach (var mail in emails)
//                        {
//                            await this.SendMail(mail,db);
//                            await db.SaveChangesAsync();
//                        }
//                    }

//                }
//                catch (Exception ex)
//                {
//                    logger.Error(ex.Message);
//                    logger.Error(ex.StackTrace);
//                }
//                finally
//                {
//                    logger.Info($"Scan completed and sent {success} email success");
//                }
                
//            }
//        }
//        private async Task<bool> SendMail(MailSendingEntity mail, DatabaseContext db)
//        {
//            try
//            {
//                logger.Info($"Begin send email for username: {mail.User.Username}");
//                using (MailMessage email = new MailMessage())
//                {
//                    using(SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com"))
//                    {
//                        email.From = new MailAddress(this.AppSettings.Email);
//                        email.To.Add(mail.User.Email);
//                        email.Subject = mail.Title;
//                        email.IsBodyHtml = true;
//                        email.Body = mail.Template;
//                        SmtpServer.UseDefaultCredentials = true;
//                        SmtpServer.Port = 587;
//                        SmtpServer.Credentials = new System.Net.NetworkCredential(this.AppSettings.Email, this.AppSettings.Secret);
//                        SmtpServer.EnableSsl = true;
//                        await SmtpServer.SendMailAsync(email);
//                        mail.Status = (int)MailStatus.Complete;
//                        mail.SentTime = DateTime.Now;
//                        db.MailSending.Update(mail);
//                    }
//                }
//                return true;
//            }
//            catch (Exception ex)
//            {
//                logger.Error(ex.Message);
//                logger.Error(ex.StackTrace);
//                return false;
//            }
//            finally
//            {
//                logger.Info("Send mail completed");
//            }
//        }
//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            while (!stoppingToken.IsCancellationRequested)
//            {
//                logger.Info("BEGIN LIFE CYCLE WORKER");
//                await this.Execute();
//                logger.Info("END LIFE CYCLE WORKER \n");
//                await Task.Delay(1000 * 10, stoppingToken);
//            }
//        }
//    }
//}
