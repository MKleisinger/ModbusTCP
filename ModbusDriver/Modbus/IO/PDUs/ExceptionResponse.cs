using System;

using Modbus.Enums;
using Modbus.Validation;

namespace Modbus.IO {
    public class ExceptionResponse {
        #region Fields

        private byte _exceptionCode = 0x01;
        private byte _errorCode = 0x00;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the response error code.
        /// 
        /// The error code is always 80 hex higher
        /// than the value of the function.
        /// 
        /// Example: 0x01 (Read Coils Function) error
        /// code would be 0x51 (0x01 + 0x50).
        /// </summary>
        public byte ErrorCode {
            get { return _errorCode; }
            set {
                if (IsValidError(value)) {
                    _errorCode = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the identifier to indicate 
        /// a particular exception error.
        /// </summary>
        public byte ExceptionCode {
            get { return _exceptionCode; }
            set {
                if (Validator.IsDefined(Activator.CreateInstance<ExceptionCode>(), value)) {
                    _exceptionCode = value;
                }
            }
        }

        #endregion

        #region Validation

        /// <summary>
        /// Determines if the proposed error code is valid.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsValidError(int code) {
            return Validator.IsDefined(Activator.CreateInstance<ExceptionCode>(), code - 0x50) || code.Equals(0x83);
        }

        #endregion
    }
}
