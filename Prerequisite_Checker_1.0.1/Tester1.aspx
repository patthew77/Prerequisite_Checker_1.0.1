<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tester1.aspx.cs" Inherits="Prerequisite_Checker_1._0._1.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            margin-left: 40px;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">
        &lt;&lt; Debug/Test Page &gt;&gt;<br />
        <br />
        Current User: (username)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblCurUser" runat="server" Text="(unnamed)"></asp:Label>
        <br />
        Change User to: (username)&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="tbChangeUser" runat="server"></asp:TextBox>
&nbsp;
        <asp:Button ID="btnChangeUser" runat="server" OnClick="btnChangeUser_Click" Text="Change" />
        <br />
        <hr />
        <br />
        1. getSubjStatus<br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Subject: (string):&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="tb1Subject" runat="server"></asp:TextBox>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Return Value: (int)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbl1Return" runat="server" Text="-"></asp:Label>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Meaning: (optional)&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbl1Meaning" runat="server" Text="-"></asp:Label>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn1Exec" runat="server" OnClick="btn1Exec_Click" Text="Execute" />
        <br />
        <br />
        <br />
        2. getSubjReq<br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Subject: (string):&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="tb2Subj" runat="server"></asp:TextBox>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; RetVal: (string[]):&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbl2RetVal" runat="server" Text="-"></asp:Label>
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; #Array_Elements:&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbl2ArraySize" runat="server" Text="-"></asp:Label>
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (^^ for confirmation)<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn2Exec" runat="server" OnClick="btn2Exec_Click" Text="Execute" />
        <br />
        <br />
        <br />
        3. getStudCur<br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn3Exec" runat="server" OnClick="btn3Exec_Click" Text="Execute" />
&nbsp;<div class="auto-style1">
            <asp:GridView ID="gv3Table" runat="server">
            </asp:GridView>
        </div>
        <br />
        <br />
        3b. getStudCurrArr<br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; RetVal: (string[]):&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbl3bRetVal" runat="server" Text="-"></asp:Label>
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Array_Size:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbl3bArrSize" runat="server" Text="-"></asp:Label>
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (^^ for confirmation)<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn3bExec" runat="server" OnClick="btn3bExec_Click" Text="Execute" />
        <br />
        <br />
        <br />
        4. getStudNo<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Student No: (int)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbl4StudNo" runat="server" Text="-"></asp:Label>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Student No: (string)&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbl4StudNoStr" runat="server" Text="-"></asp:Label>
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn4Exec" runat="server" OnClick="btn4Exec_Click" Text="Execute" />
        <br />
        <br />
        <br />
        5. getStudName<br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Student Name:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbl5StudName" runat="server" Text="-"></asp:Label>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn5Exec" runat="server" OnClick="btn5Exec_Click" Text="Execute" />
        <br />
        <br />
        <br />
        <hr />
        <br />
        Database Tables:&nbsp;
        <br />
        &nbsp;<asp:Button ID="btnUpdateTableList" runat="server" Text="Update (?)" />
        <br />
        <asp:GridView ID="gvTableList" runat="server">
        </asp:GridView>
        <br />
        <br />
        <br />
        <asp:Label ID="Label1" runat="server" Text="View Which Table?"></asp:Label>
        &nbsp;
        <asp:TextBox ID="tbTableName" runat="server"></asp:TextBox>
        &nbsp;
        <asp:Button ID="btnGetTable" runat="server" Text="Get Table" OnClick="btnGetTable_Click" />
        <br />
        <br />
        Current Table: <asp:Label ID="lblCurTable" runat="server" Text="samp_cur"></asp:Label>
        <br />
        &nbsp;<asp:GridView ID="gv1" runat="server">
        </asp:GridView>
        <br />
        <br />
        Error Message(s):<br />
        <asp:Label ID="lblErr1" runat="server" Text="-"></asp:Label>
        <br />
        <asp:Label ID="lblErr2" runat="server" Text="-"></asp:Label>
        <br />
        <asp:Label ID="lblErr3" runat="server" Text="-"></asp:Label>
        <br />
        <asp:Label ID="lblErr4" runat="server" Text="-"></asp:Label>
    </form>
</body>
</html>
