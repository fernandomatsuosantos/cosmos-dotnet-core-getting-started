<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SqlInjection.aspx.cs" Inherits="SqlInjectionExample.SqlInjection" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AspnetO.com | Simple Test SQL Injection Example in Asp.net</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h4>Simple Test SQL Injection Example in Asp.net</h4>
            <table>
                <tr>
                    <td>UserName:
                    </td>
                    <td>
                        <asp:TextBox ID="txtUserName" runat="server" Width="100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnWithSqlInjection" runat="server" Text="With Injection" OnClick="btnWithSqlInjection_Click" />
                        &nbsp;
                    <asp:Button ID="btnWithProtection" runat="server" Text="With Protection" OnClick="btnWithProtection_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
