package net.ravendb.northwind;

import java.util.Date;
import java.util.List;

public class Employee {

    private String id;
    private String lastName;
    private String firstName;
    private String title;
    private Address address;
    private Date hiredAt;
    private Date birthday;
    private String homePhone;
    private String extension;
    private String reportsTo;
    private List<String> notes;
    private List<String> territories;

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public Address getAddress() {
        return address;
    }

    public void setAddress(Address address) {
        this.address = address;
    }

    public Date getHiredAt() {
        return hiredAt;
    }

    public void setHiredAt(Date hiredAt) {
        this.hiredAt = hiredAt;
    }

    public Date getBirthday() {
        return birthday;
    }

    public void setBirthday(Date birthday) {
        this.birthday = birthday;
    }

    public String getHomePhone() {
        return homePhone;
    }

    public void setHomePhone(String homePhone) {
        this.homePhone = homePhone;
    }

    public String getExtension() {
        return extension;
    }

    public void setExtension(String extension) {
        this.extension = extension;
    }

    public String getReportsTo() {
        return reportsTo;
    }

    public void setReportsTo(String reportsTo) {
        this.reportsTo = reportsTo;
    }

    public List<String> getNotes() {
        return notes;
    }

    public void setNotes(List<String> notes) {
        this.notes = notes;
    }

    public List<String> getTerritories() {
        return territories;
    }

    public void setTerritories(List<String> territories) {
        this.territories = territories;
    }
}
