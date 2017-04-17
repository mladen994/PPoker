using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using ServiceStack.Text;

namespace PPoker {
    class RedisTester {
        readonly RedisClient redis = new RedisClient(Config.SingleHost);

        RedisTester() {
            //redis. asdfasdasda
        }
    }
}
