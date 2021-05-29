using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Qrame.CoreFX.ExtensionMethod;

namespace Qrame.CoreFX.Diagnostics
{
    /// <summary>
    /// SQL Server(2005 버전 이상)에서 제공되는 RetrieveStatistics 메서드의 결과를 저장할 엔터티 클래스입니다.
    /// </summary>
    public class SqlServerStatisticsInfo
    {
        /// <summary>
        /// 인스턴스 생성시 RetrieveStatistics 메서드의 결과를 엔터티 클래스에 저장합니다.
        /// </summary>
        public SqlServerStatisticsInfo(IDictionary statistics)
        {
            BytesReceived = statistics.Get<long>("BytesReceived");
            BytesSent = statistics.Get<long>("BytesSent");
            ConnectionTime = statistics.Get<long>("ConnectionTime");
            ExecutionTime = statistics.Get<long>("ExecutionTime");
            IduCount = statistics.Get<long>("IduCount");
            IduRows = statistics.Get<long>("IduRows");
            SelectCount = statistics.Get<long>("SelectCount");
            SelectRows = statistics.Get<long>("SelectRows");
            NetworkServerTime = statistics.Get<long>("NetworkServerTime");
            ServerRoundtrips = statistics.Get<long>("ServerRoundtrips");
            SumResultSets = statistics.Get<long>("SumResultSets");
            Transactions = statistics.Get<long>("Transactions");
        }

        /// <summary>
        /// RetrieveStatistics 정보의 스냅샷을 가지고 있는 타이머의 식별자입니다.
        /// </summary>
        internal Guid RecordTimerID;

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 SQL Server에서 공급자가 받은 TDS 패킷의 데이터 바이트 수입니다.
        /// </summary>
        private long bytesReceived = 0;

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 SQL Server에서 공급자가 받은 TDS 패킷의 데이터 바이트 수를 반환합니다.
        /// </summary>
        public long BytesReceived
        {
            get { return bytesReceived; }
            set { bytesReceived = value; }
        }

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 TDS 패킷의 SQL Server로 전송된 데이터 바이트 수입니다.
        /// </summary>
        private long bytesSent = 0;

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 TDS 패킷의 SQL Server로 전송된 데이터 바이트 수를 반환합니다.
        /// </summary>
        public long BytesSent
        {
            get { return bytesSent; }
            set { bytesSent = value; }
        }

        /// <summary>
        /// 통계가 활성화된 후 연결이 열려 있던 시간입니다.
        /// </summary>
        private long connectionTime = 0;

        /// <summary>
        /// 통계가 활성화된 후 연결이 열려 있던 시간입니다(연결을 열기 전에 통계를 활성화한 경우에는 총 연결 시간).
        /// </summary>
        public long ConnectionTime
        {
            get { return connectionTime; }
            set { connectionTime = value; }
        }

        /// <summary>
        /// 통계가 활성화된 후 서버의 응답을 기다린 시간과 공급자 자체에서 코드를 실행하는 데 걸린 시간을 포함하여 공급자에서 처리에 소요된 누적 시간입니다.
        /// </summary>
        private long executionTime = 0;

        /// <summary>
        /// 통계가 활성화된 후 서버의 응답을 기다린 시간과 공급자 자체에서 코드를 실행하는 데 걸린 시간을 포함하여 공급자에서 처리에 소요된 누적 시간을 반환합니다.
        /// </summary>
        public long ExecutionTime
        {
            get { return executionTime; }
            set { executionTime = value; }
        }

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 연결을 통해 실행되는 INSERT, DELETE 및 UPDATE 문의 총 수입니다.
        /// </summary>
        private long iduCount = 0;

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 연결을 통해 실행되는 INSERT, DELETE 및 UPDATE 문의 총 수를 반환합니다.
        /// </summary>
        public long IduCount
        {
            get { return iduCount; }
            set { iduCount = value; }
        }

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 연결을 통해 실행되는 INSERT, DELETE 및 UPDATE 문에 영향을 받은 전체 행 수입니다.
        /// </summary>
        private long iduRows = 0;

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 연결을 통해 실행되는 INSERT, DELETE 및 UPDATE 문에 영향을 받은 전체 행 수를 반환합니다.
        /// </summary>
        public long IduRows
        {
            get { return iduRows; }
            set { iduRows = value; }
        }

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 연결을 통해 실행된 SELECT 문의 수입니다.
        /// </summary>
        private long selectCount = 0;

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 연결을 통해 실행된 SELECT 문의 수를 반환합니다. 여기에는 커서에서 행을 검색하는 FETCH 문이 포함되며 SqlDataReader의 끝에 도달하면 SELECT 문의 수가 업데이트됩니다.
        /// </summary>
        public long SelectCount
        {
            get { return selectCount; }
            set { selectCount = value; }
        }

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 선택된 행 수입니다.
        /// </summary>
        private long selectRows = 0;

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 선택된 행 수를 반환합니다. 이 카운터는 호출자가 실제로 사용하지 않은 행을 포함해서 SQL 문에서 생성된 모든 행을 나타냅니다. 예를 들어, 전체 결과 집합을 읽기 전에 데이터 판독기를 닫으면 개수가 달라지지 않습니다. 여기에는 FETCH 문을 통해 커서에서 검색한 행도 포함됩니다.
        /// </summary>
        public long SelectRows
        {
            get { return selectRows; }
            set { selectRows = value; }
        }

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 공급자에서 서버의 응답을 기다리는 데 소요된 누적 시간의 양입니다.
        /// </summary>
        private long networkServerTime = 0;

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 공급자에서 서버의 응답을 기다리는 데 소요된 누적 시간의 양을 반환합니다.
        /// </summary>
        public long NetworkServerTime
        {
            get { return networkServerTime; }
            set { networkServerTime = value; }
        }

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 연결에서 서버로 명령을 보내고 응답을 받은 횟수입니다.
        /// </summary>
        private long serverRoundtrips = 0;

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 연결에서 서버로 명령을 보내고 응답을 받은 횟수를 반환합니다.
        /// </summary>
        public long ServerRoundtrips
        {
            get { return serverRoundtrips; }
            set { serverRoundtrips = value; }
        }

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 사용된 결과 집합의 수입니다.
        /// </summary>
        private long sumResultSets = 0;

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 사용된 결과 집합의 수를 반환합니다. 예를 들어, 여기에는 클라이언트로 반환된 모든 결과 집합이 포함됩니다. 커서의 경우 각 반입 또는 블록 반입 작업은 개별 결과 집합으로 간주됩니다.
        /// </summary>
        public long SumResultSets
        {
            get { return sumResultSets; }
            set { sumResultSets = value; }
        }

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 시작된 사용자 트랜잭션의 수입니다.
        /// </summary>
        private long transactions = 0;

        /// <summary>
        /// 응용 프로그램에서 공급자 사용을 시작하고 통계를 활성화한 후 시작된 사용자 트랜잭션의 수를 반환합니다. 자동 커밋 기능이 작동되는 상태에서 연결을 실행하면 각 명령은 트랜잭션으로 간주됩니다. 이 카운터는 트랜잭션이 나중에 커밋되든 롤백되든 상관없이 BEGIN TRAN 문이 실행되는 즉시 트랜잭션 수를 증가시킵니다.
        /// </summary>
        public long Transactions
        {
            get { return transactions; }
            set { transactions = value; }
        }
    }
}
