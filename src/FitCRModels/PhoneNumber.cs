namespace FitCRModels
{
    /// <summary>
    /// Class to define the phone number structure.
    /// </summary>
    public class PhoneNumber
    {
        private const int DefaultCode = 506;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneNumber"/> class.
        /// </summary>
        /// <param name="number">The number. It can be only the digits or "+code digits".</param>
        public PhoneNumber(string number)
        {
            this.Code = DefaultCode;

            if (string.IsNullOrWhiteSpace(number))
            {
                return;
            }

            // Si viene con el signo + es porque trae el codigo de region separado por un espacio.
            if (number.StartsWith('+'))
            {
                var parts = number.Split(' ');
                this.Code = int.Parse(parts[0].Substring(1));
                this.Digits = int.Parse(parts[1]);
            }
            else
            {
                this.Digits = int.Parse(number);
            }
        }

        /// <summary>
        /// Gets or sets the region code.
        /// </summary>
        /// <value>
        /// The region code.
        /// </value>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the digits of the number.
        /// </summary>
        /// <value>
        /// The digits of the number.
        /// </value>
        public int Digits { get; set; }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"+{this.Code} {this.Digits}";
        }
    }
}
