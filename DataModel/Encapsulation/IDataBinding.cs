using System.Data;

namespace Qrame.CoreFX.DataModel
{
    /// <summary>
    /// 엔터티 타입에 데이터 바인들을 수행할 인터페이스 정의입니다.
    /// </summary>
    public interface IDataBinding
    {
        /// <summary>
        /// 앞으로만 이동 가능한 스트림에서 데이터 바인딩을 수행후 결과를 반환합니다.
        /// </summary>
        /// <param name="dataReader">앞으로만 이동 가능한 스트림입니다.</param>
        /// <returns>데이터 바인딩을 수행후 런타임에 확인할 결과입니다.</returns>
        dynamic BindingData(IDataReader dataReader);
    }
}
