using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SACS.Library.Configuration
{
    /// <summary>
    /// The configuration provider that reads settings from files.
    /// </summary>
    public class SACSConfigProvider : IConfigProvider
    {
        /// <summary>
        /// The comment character (if line starts with it, will be ignored)
        /// </summary>
        private const string CommentChar = "#";

        /// <summary>
        /// The properties dictionary mapping names to values.
        /// </summary>
        private readonly Dictionary<string, string> properties = new Dictionary<string, string>();

        /// <summary>
        /// The configuration file path
        /// </summary>
        private readonly string configPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="SACSConfigProvider"/> class.
        /// </summary>
        /// <param name="configPath">The path of the configuration file.</param>
        public SACSConfigProvider(string configPath)
        {
            this.configPath = configPath;
            this.ReadFromFile();
        }

        /// <summary>
        /// Gets the REST group setting or SOAP organization setting.
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
        public string Group
        {
            get { return this.GetProperty("group"); }
        }

        /// <summary>
        /// Gets the REST user identifier or SOAP username.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId
        {
            get { return this.GetProperty("userId"); }
        }

        /// <summary>
        /// Gets the REST client secret or SOAP password.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string ClientSecret
        {
            get { return this.GetProperty("clientSecret"); }
        }

        /// <summary>
        /// Gets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        public string Domain
        {
            get { return this.GetProperty("domain"); }
        }

        /// <summary>
        /// Gets the environment URL address.
        /// </summary>
        /// <value>
        /// The environment URL address.
        /// </value>
        public string Environment
        {
            get { return this.GetProperty("environment"); }
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <param name="property">The property name.</param>
        /// <returns>Value of the property.</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Property is not present in the file.</exception>
        private string GetProperty(string property)
        {
            string value;
            bool present = this.properties.TryGetValue(property, out value);
            if (present)
            {
                return value;
            }
            else
            {
                string msg = string.Format("Property {0} is not present in file: {1}", property, this.configPath);
                throw new KeyNotFoundException(msg);
            }
        }

        /// <summary>
        /// Reads the properties from file.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Configuration file not found.</exception>
        private void ReadFromFile()
        {
            if (!File.Exists(this.configPath))
            {
                throw new FileNotFoundException("Configuration file not found: " + this.configPath);
            }

            Regex configLineRegex = new Regex(@"^([a-zA-Z]+)=(.+)$");
            using (StreamReader reader = new StreamReader(this.configPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.StartsWith(CommentChar))
                    {
                        continue;
                    }

                    Match match = configLineRegex.Match(line);
                    if (match.Groups.Count < 3)
                    {
                        continue;
                    }

                    string left = match.Groups[1].Value.Trim();
                    string right = match.Groups[2].Value.Trim();
                    this.properties[left] = right;
                }
            }
        }
    }
}