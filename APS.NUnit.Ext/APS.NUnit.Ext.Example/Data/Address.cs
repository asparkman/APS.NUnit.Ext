//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

public partial class Address
{
    public Address()
    {
        this.People = new HashSet<Person>();
    }

    public int Id { get; set; }
    public string StreetAddress { get; set; }
    public string StreetAddress2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }

    public virtual ICollection<Person> People { get; set; }
}
