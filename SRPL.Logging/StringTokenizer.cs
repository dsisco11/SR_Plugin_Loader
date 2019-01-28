using System;
using System.Collections.Generic;
using System.Text;

namespace SRPL.Logging
{
    public class StringTokenizer
    {
        private int c = 0;// The current position a Consume() call will begin from
        private int p = 0;// The current position the next Peek() call will begin from
        private string buf;

        public StringTokenizer(string str)
        {
            buf = str;
        }

        /// <summary>
        /// Returns the string value of the area between the end of the previously consumed result and the current peek index
        /// </summary>
        /// <returns></returns>
        public string Consume()
        {
            // If we DON'T increment the peek index in this function then it will not make logical sense, without incrementing the peek index each consecutive call to Consume() will give a result which begins with the last char that was included in the previous call!
            this.Next();
            // Get the segment we just consumed (the area from the last consume idx and the current idx we are peeking at
            string res = buf.Substring(c, p - c);

            // Now we move the consume idx to the peek idx
            c = p;

            // Lastly return the segment we consumed
            return res;
        }

        /// <summary>
        /// Attempts to consume a specified character and returns true if successful.
        /// </summary>
        /// <param name="ch">The character to consume</param>
        /// <returns>True/False if the character was present and able to be consumed</returns>
        public bool TryConsume(char ch)
        {
            if (this.peek() != ch) return false;

            this.Consume();
            return true;
        }

        /// <summary>
        /// Consumes and returns the char at the current consume index, the increments the consume index by 1
        /// </summary>
        public string ConsumeNext()
        {
            if (++c > p) p = c;// Increment the consume index and if it is then greater than the peek index update it also.            
            return buf.Substring(c - 1, 1);
        }

        /// <summary>
        /// Let's us view the current char that would be dedicated to the consume list of we called Next()
        /// </summary>
        public char peek()
        {
            return buf[p];
        }

        /// <summary>
        /// Let's us peek at the next char without really dedicating to it.
        /// Allows us to tell if the next char fits a particular criteria before calling next() and causing it to then become part of the segment which will be returned by a call to Consume()
        /// </summary>
        public char peekNext()
        {
            if (p + 1 >= buf.Length) return default(char);
            return buf[p + 1];
        }

        /// <summary>
        /// Increases the peek index by 1 if possible and returns if it was possible.
        /// </summary>
        public bool Next()
        {
            if (p + 1 > buf.Length) return false;

            p += 1;
            return true;
        }

        /// <summary>
        /// Moves the current peek index to the end of the buffer
        /// </summary>
        public void PeekEnd()
        {
            p = (buf.Length - 1);
        }

        /// <summary>
        /// Consumes the rest of the string and returns the result.
        /// </summary>
        public string ConsumeAll()
        {
            p = buf.Length;
            return this.Consume();
        }

        /// <summary>
        /// Returns wether or not the current peek index is at the end of the buffer
        /// </summary>
        public bool CanPeek()
        {
            return (p + 1 <= buf.Length);
        }

        /// <summary>
        /// Returns wether or not calling Next would return True
        /// </summary>
        public bool HasNext()
        {
            return !(p + 1 > buf.Length);
        }

        /// <summary>
        /// Returns wether or not the current peek index is at the end of the buffer
        /// </summary>
        public bool CanConsume()
        {
            return (c + 1 >= buf.Length);
        }

        /// <summary>
        /// Resets the peek index back to 
        /// </summary>
        public void ResetPeek()
        {
            p = c;
        }

        /// <summary>
        /// Moves the peek index back by a specified amount (Defaults to 1)
        /// </summary>
        public void Rewind(int r = 1)
        {
            p -= r;
            if (p < 0) p = 0;
            if (p < c) c = p;
        }
    }

}
