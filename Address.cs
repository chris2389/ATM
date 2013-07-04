using System;

public class Address
{
    private String street;
    private String city;
    private String state;
    private String zip;
    public Address(String st, String cy, String se, String zp) {
        street =st;
        city = cy;
        state = se;
        zip = zp;
    }
    public override string  ToString(){
        return street + "\n" + city + ", " + state + " "+ zip;
    }
}
	

