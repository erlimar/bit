// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.Linq;
using System.Threading.Tasks;

namespace E5R.Tools.Bit
{
    using Sdk.Bit.Command;
    using Sdk.Bit.Services.Abstractions;
    using Engine;
    using Engine.DI;

    internal class Program
    {
        private readonly BitEngine _engine;

        internal Program(DependencyInjectionContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            _engine = new BitEngine(container);
        }

        internal async Task<BitResult> Run()
        {
            return new BitResult
            {
                ResultCode = -1
            };
        }

        internal static int Main()
        {
            // TODO: Migrate to Main async method with C# 7.1

            BitResult result = null;

            try
            {
                var container = DependencyInjectionContainer.BuildDefault();
                var program = new Program(container);
                var task = program.Run();

                task.Wait();

                if (!task.IsCompletedSuccessfully)
                {
                    throw task.Exception;
                }

                result = task.Result;
            }
            catch (Exception exception)
            {
                result = new BitResult
                {
                    ResultCode = exception.HResult,
                    ResultMessage = exception.Message
                };
            }
            finally
            {
                if (result == null)
                {
                    result = new BitResult
                    {
                        ResultCode = -1,
                        ResultMessage = "Fatal unespected exception."
                    };
                }
            }

            // TODO: Print messages
            return result.ResultCode;
        }
    }
}
