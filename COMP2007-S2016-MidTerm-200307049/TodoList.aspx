<%@ Page Title="Todo List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TodoList.aspx.cs" Inherits="COMP2007_S2016_MidTerm_200307049.TodoList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-offset-1 col-md-8">
                <h3>Todo List</h3>
                <a href="/TodoDetails.aspx" class="btn btn-success btn-sm"><i class="fa fa-plus"></i>Add a Todo</a>
                <asp:GridView ID="TodoGridView" AllowPaging="true" PageSize="3" runat="server" CssClass="table table-bordered table-striped table-hover"
                    AutoGenerateColumns="false" DataKeyNames="TodoID" OnRowDeleting="TodoGridView_RowDeleting">

                    <Columns>
                        <asp:TemplateField HeaderText="No.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="TodoID" Visible="false" />
                        <asp:BoundField DataField="TodoName" HeaderText="Name" Visible="true" />
                        <asp:BoundField DataField="TodoNotes" HeaderText="Notes" Visible="true" />
                        <asp:CheckBoxField DataField="Completed" HeaderText="Completed" Visible="true" />

                        <asp:HyperLinkField HeaderText="Edit" Text="<i calss='fa fa-encil-square-o fa-lg'></i> Edit"
                            NavigateUrl="/TodoDetails.aspx" ControlStyle-CssClass="btn btn-primary btn-sm" DataNavigateUrlFields="TodoID"
                            runat="server" DataNavigateUrlFormatString="/TodoDetails.aspx?TodoID={0}" />
                        <asp:CommandField HeaderText="Delete" DeleteText="<i class='fa fa-trash-o fa-lg'></i> Delete" ShowDeleteButton="true" ButtonType="Link"
                            ControlStyle-CssClass="btn btn-danger btn-sm" />

                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div>
            <label for="PageSizeDropDownList"></label>
            <asp:DropDownList runat="server" ID="PageSizeDropDownList" AutoPostBack="true"
                CssClass="btn btn-default bt-sm dropdown-toggle" OnSelectedIndexChanged="PageSizeDropDownList_SelectedIndexChanged">
                <asp:ListItem Text="3" Value="3" />
                <asp:ListItem Text="5" Value="5" />
                <asp:ListItem Text="10" Value="10" />
                <asp:ListItem Text="All" Value="10000" />
            </asp:DropDownList>
        </div>
    </div>
</asp:Content>
