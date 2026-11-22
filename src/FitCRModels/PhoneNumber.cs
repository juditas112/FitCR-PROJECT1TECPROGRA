namespace FitCRModels
{
    /// <summary>Class to define the phone number structure.</summary>
    public class PhoneNumber
    {
        private const int DefaultCode = 506;

        public PhoneNumber(string number)
        {
            this.Code = DefaultCode;

            if (string.IsNullOrWhiteSpace(number))
            {
                return;
            }

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

        public int Code { get; set; }

        public int Digits { get; set; }

        public override string ToString()
        {
            return $"+{this.Code} {this.Digits}";
        }
    }
}
