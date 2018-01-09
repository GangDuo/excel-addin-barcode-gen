using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace BarcodeGen.Settings
{
    public class ForWritingViewModel
    {
        [Required(ErrorMessage = "数字を入力してください")]
        public ReactiveProperty<string> Height { get; private set; }

        [Required(ErrorMessage = "数字を入力してください")]
        public ReactiveProperty<string> Width { get; private set; }

        [Required(ErrorMessage = "数字を入力してください")]
        public ReactiveProperty<string> Margin { get; private set; }

        public ReactiveCommand ApplyCommand { get; private set; }

        public ForWritingViewModel()
        {
            // 未入力または数字以外はエラー
            Func<string, string> validator = (x) =>
            {
                int n;
                return (string.IsNullOrWhiteSpace(x) || !int.TryParse(x, out n)) ? "Error!!" : null;
            };

            Height = new ReactiveProperty<string>("40")
                .SetValidateNotifyError(validator);

            Width = new ReactiveProperty<string>("200")
                .SetValidateNotifyError(validator);

            Margin = new ReactiveProperty<string>("30")
                .SetValidateNotifyError(validator);

            this.ApplyCommand = new[]
            {
                Height.ObserveHasErrors,
                Width.ObserveHasErrors,
                Margin.ObserveHasErrors
            }
            // 全てFalseだったら
            .CombineLatestValuesAreAllFalse()
            .ToReactiveCommand();

            // Executeが呼ばれたらインクリメント
            this.ApplyCommand
                .Subscribe(_ => Debug.WriteLine(""));

        }
    }
}
