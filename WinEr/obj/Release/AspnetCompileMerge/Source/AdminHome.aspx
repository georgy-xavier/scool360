<%@ Page Language="C#" MasterPageFile="~/AdminHomeMaster.master" AutoEventWireup="true" Inherits="AminHome"  Codebehind="AdminHome.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="main">
	
		<div class="left">

			<div class="content">

				<h1>
                    WinEr Manager</h1>

				<p>
                    This application will help you to manage the Group,User and Type</p>

				<h1>WinEr</h1>
			<p>
			WinEr  is a School Management Software developed by Winceron Software Technologies Private Limited,  It is able to help the management to ease their work in the area of school management , it can reconcile reports of Student performance, Fees, and other required certificates , and. Management can track a student by name and id for the details of fees and performance record. 
			</p>
			
			<p>
			Winceron delivers you a state of the art and the most comprehensive school management solution through WinEr. 
			</p>
			</div>

		</div>

		<div class="right">

			<div class="subnav">

				<h1>Group</h1>
				<ul>
					<li><a href="CreateGroup.aspx">Create Group</a></li><li><a href="ManageGroup.aspx">Manage Group</a></li><li><a href="DeleteGroup.aspx">Delete Group</a></li><li><a href="AddMembers.aspx">Manage Members</a></li></ul>
					<h1>
                        Role</h1>
				<ul>
					<li><a href="CreateRole.aspx">Create Role</a></li><li><a href="ManageRole.aspx">Manage Role</a></li><li><a href="DeleteRole.aspx">Delete Role</a></li></ul>

				<h1>User</h1>
				<ul>
					<li><a href="CreateUser.aspx">Create User</a></li><li><a href="ManageUser.aspx">Manage User</a></li></ul>

				

			</div>

		</div>

		<div class="clearer"><span></span></div>

	</div>

</asp:Content>

