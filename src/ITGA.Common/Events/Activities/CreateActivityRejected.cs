using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace ITGA.Common.Events.Activities
{
    public class CreateActivityRejected
    {
        public Guid CommandId {get;}
        public string Reason { get; }
        public string Code { get; }

        protected CreateActivityRejected()
        {

        }

        public CreateActivityRejected(Guid commandId, string code, string reason)
        {
            CommandId = commandId;
            Code = code;
            Reason = reason;
        }
    }
}
