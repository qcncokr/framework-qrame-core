using System;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

namespace Qrame.CoreFX
{
    /// <summary>
    /// LateBoundingMethod 메서드는 제네릭 메서드로 인스턴스와 매개 변수 목록을 전달하여 호출 가능
    /// 하도록 해주는 기본적인 기능을 제공 합니다. (닷넷 프레임워크 4.0의 dynamic 키워드로 인해 사용 안할 예정입니다.)
    /// </summary>
    /// <param name="boundingTarget">동적으로 호출되는 메서드의 호출 정보를 포함하는 타입입니다.</param>
    /// <param name="boundingArguments">메서드의 매개 변수 목록입니다.</param>
    /// <returns>모든 반환 결과를 object 타입으로 반환합니다.</returns>
    public delegate object LateBoundingMethod(object boundingTarget, object[] boundingArguments);

    /* 전체사용샘플
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            MethodInfo Method = typeof(Form1).GetMethod("ThreadTimer_Tick", new[] { typeof(object), typeof(EventArgs) });
            LateBoundingMethod LateMethod = DelegateFactory.Create(Method);

            string DateText = (string)LateMethodTest(LateMethod);
        }

        public object LateMethodTest(LateBoundingMethod BoundingMethod)
        {
            return BoundingMethod(this, new[] { this, null });
        }

        public object ThreadTimer_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString();
            return label1.Text;
        }
    }
     */
    
    /// <summary>
    /// MethodInfo 타입 정보를 이용 하여, 어디에서든 호출 가능하도록 제네릭 메서드 델리게이트를 생성합니다. 닷넷 프레임워크 4.0 이상 버전에서 dynamic 키워드로 인해, 선별적으로 사용 예정입니다.
    /// <code>
    /// MethodInfo Method = typeof(String).GetMethod("StartsWith", new[] { typeof(string) });
    /// LateBoundingMethod callback = DelegateFactory.Create(Method);
    ///
    /// string foo = "테스트 문자열입니다.";
    /// bool result = (bool) callback(foo, new[] { "문자열" }); // 결과 : true
    /// </code>
    /// </summary>
    public static class DelegateFactory
    {
        /// <summary>
        /// MethodInfo 타입 정보를 이용 하여, LateBoundingMethod 델리게이트를 생성합니다.
        /// </summary>
        /// <param name="methodInfo">MethodInfo 타입입니다.</param>
        /// <returns>LateBoundingMethod 델리게이트입니다.</returns>
        public static LateBoundingMethod Create(MethodInfo methodInfo)
        {
            ParameterExpression instanceParameter = Expression.Parameter(typeof(object), "LateBoundingTarget");
            ParameterExpression argumentsParameter = Expression.Parameter(typeof(object[]), "LateBoundingArguments");

            MethodCallExpression call = Expression.Call(Expression.Convert(instanceParameter, methodInfo.DeclaringType), methodInfo, CreateParameterExpressions(methodInfo, argumentsParameter));

            Expression<LateBoundingMethod> lambda = Expression.Lambda<LateBoundingMethod>(Expression.Convert(call, typeof(object)), instanceParameter, argumentsParameter);
            
            return lambda.Compile();
        }

        /// <summary>
        /// 전통적인 방식으로 타입, 메서드명, 매개 변수 타입 목록을 이용 하여, LateBoundingMethod 델리게이트를 생성합니다.
        /// </summary>
        /// <param name="objectType">동적으로 호출할 메서드의 선언이 포함되어 있는 타입 정보입니다.</param>
        /// <param name="methodName">동적으로 호출할 메서드명입니다.</param>
        /// <param name="parameterTypes">동적으로 호출할 메서드의 매개 변수 타입 목록입니다.</param>
        /// <returns>LateBoundingMethod 타입입니다.</returns>
        public static LateBoundingMethod Create(Type objectType, string methodName, params Type[] parameterTypes)
        {
            return Create(objectType.GetMethod(methodName, parameterTypes));
        }

        /// <summary>
        /// 메서드 매개 변수 타입 목록 정보를 포함하는 식 트리를 반환합니다.
        /// </summary>
        /// <param name="methodInfo">MethodInfo 타입입니다.</param>
        /// <param name="argumentsParameter">Expression 식 정보입니다.</param>
        /// <returns>Expression[] 타입입니다.</returns>
        private static Expression[] CreateParameterExpressions(MethodInfo methodInfo, Expression argumentsParameter)
        {
            return methodInfo.GetParameters().Select((parameter, index) => Expression.Convert(Expression.ArrayIndex(argumentsParameter, Expression.Constant(index)), parameter.ParameterType)).ToArray();
        }

        public delegate void LateBoundingVoid(object target, object[] arguments);

        public static LateBoundingVoid CreateVoid(MethodInfo method)
        {
            ParameterExpression instanceParameter = Expression.Parameter(typeof(object), "LateBoundingTarget");
            ParameterExpression argumentsParameter = Expression.Parameter(typeof(object[]), "LateBoundingArguments");

            MethodCallExpression call = Expression.Call(
                Expression.Convert(instanceParameter, method.DeclaringType),
                method,
                CreateParameterExpressions(method, argumentsParameter));

            var lambda = Expression.Lambda<LateBoundingVoid>(
                Expression.Convert(call, method.ReturnParameter.ParameterType),
                instanceParameter,
                argumentsParameter);

            return lambda.Compile();
        }
    }
}
