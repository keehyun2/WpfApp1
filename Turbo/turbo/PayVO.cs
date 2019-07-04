using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

namespace Turbo.turbo
{
    class PayVO
    {
        public ObjectId Id { get; set; }
        public long? No { get; set; } // 번호
        public String Worker { get; set; } // 작업자
        public String CompNum { get; set; } // 사업자번호
        public long? Amt { get; set; } // 금액
        public String PayDt { get; set; } // 지급일
        public String Company { get; set; } // 업체명
        public String WriteDt { get; set; } // 작성일
    }
}
