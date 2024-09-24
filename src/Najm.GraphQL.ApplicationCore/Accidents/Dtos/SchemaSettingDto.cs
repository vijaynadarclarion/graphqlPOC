using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Najm.GraphQL.ApplicationCore.Accidents.Dtos;
public class AuthorizationConfig
{
    public List<AuthorizationRule> Authorization { get; set; }
}

public class AuthorizationRule
{
    public string Name { get; set; }

    public bool Value { get; set; }
}
