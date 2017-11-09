using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public interface ISerializable
    {

        byte[] Serialize();
        void Deserialize(byte[] Serialized);

        bool isNetworkDirty();
        void setNetworkClean(bool clean);

    }
}
