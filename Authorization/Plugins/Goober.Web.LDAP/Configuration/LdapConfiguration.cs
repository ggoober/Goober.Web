using Goober.Web.Authorization.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goober.Web.LDAP.Configuration
{
    public class LdapConfiguration
    {
        /// <summary>
        /// Адрес сервера LDAP
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Порт на сервере LDAP
        /// </summary>
        public int Port { get; set; } = 389;

        /// <summary>
        /// Общее имя пользователя (CN)
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// Зашифрованный пароль пользователя
        /// </summary>
        public string PasswordEncrypted { get; set; } = string.Empty;

        /// <summary>
        ///  Компонент каталога отличительного имени пользователя (distinguishedName).
        /// </summary>
        public string UsersCommonName { get; set; } = string.Empty;

        /// <summary>
        /// Компонент домена отличительного имени пользователя (distinguishedName).
        /// </summary>
        public string DomainComponent { get; set; } = string.Empty;

        /// <summary>
        /// Фильтр для поиска пользователя (опционально)
        /// В фильтре параметр <see cref="Glossaries.LdapFilterParamGlossary.UserName"/>заменяется на фактическое имя пользователя
        /// </summary>
        public string? FilterSearchUser { get; set; } = string.Empty;
        [ConfigurationKeyName("Cookie")]
        public CookiesOptions? CookiesOptions { get; set; }
    }
}
