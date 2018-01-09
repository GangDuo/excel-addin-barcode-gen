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
    class ForWritingViewModel
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

            Height = new ReactiveProperty<string>(Config.Instance.Height.ToString())
                .SetValidateNotifyError(validator);

            Width = new ReactiveProperty<string>(Config.Instance.Width.ToString())
                .SetValidateNotifyError(validator);

            Margin = new ReactiveProperty<string>(Config.Instance.Margin.ToString())
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

            this.ApplyCommand
                .Subscribe(_ =>
                {
                    Config.Instance.Height = int.Parse(Height.Value);
                    Config.Instance.Width = int.Parse(Width.Value);
                    Config.Instance.Margin = int.Parse(Margin.Value);
                });

        }
    }
}
