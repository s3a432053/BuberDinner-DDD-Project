using ErrorOr;
using FluentValidation;
using MediatR;

namespace BuberDinner.Application.Common.Behaviors
{
    /// <summary>
    /// 資料驗證 Class 繼承 MediaR 套件的 IPipelineBehavior 的話 流程在進入 Handler 之前就會先進到這裡
    /// </summary>
    /// <typeparam name="TRequest"> 傳入的 TRequest 必須是有實作 IRequest<TResponse> 的實體 </typeparam>
    /// <typeparam name="TResponse"> 回傳的 TResponse 必須是有實作 IErrorOr 的實體 </typeparam>
    public class ValidationBehavior<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
    {
        private readonly IValidator<TRequest>? _validator;

        /// <summary>
        /// 不一定所有的 Controller 都會使用 validator 所以預設 null
        /// </summary>
        /// <param name="validator"></param>
        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        /// <summary>
        /// 處理資料驗證
        /// </summary>
        /// <param name="request">要驗證的 Request 資料</param>
        /// <param name="next">驗證通過 要觸發的 Handler 委派方法 回傳值是 TResponse</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (_validator is null)
            {
                // 執行 Handler
                return await next();
            }

            // 檢查資料合法性
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // 合法 => 執行 Handler
            if (validationResult.IsValid)
            {
                return await next();
            }

            // 轉換成 List<Error>
            var errors = validationResult.Errors
                .ConvertAll(validationFailure => Error.Validation(
                    validationFailure.PropertyName, 
                    validationFailure.ErrorMessage
                ));

            // dynamic => 系統在執行的時候會自動檢查 List<Error> 是否能被轉換成 TResponse
            return (dynamic)errors;
        }
    }
}
