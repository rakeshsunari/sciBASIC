﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("Microsoft.VisualBasic.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Byte[].
        '''</summary>
        Friend ReadOnly Property bashRunner() As Byte()
            Get
                Dim obj As Object = ResourceManager.GetObject("bashRunner", resourceCulture)
                Return CType(obj,Byte())
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to ?&lt;html&gt;
        '''&lt;head&gt;
        '''&lt;title&gt;The Code Project Open License (CPOL)&lt;/title&gt;
        '''&lt;Style&gt;
        '''BODY, P, TD { font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 10pt }
        '''H1,H2,H3,H4,H5 { color: #ff9900; font-weight: bold; }
        '''H1 { font-size: 14pt;color:black }
        '''H2 { font-size: 13pt; }
        '''H3 { font-size: 12pt; }
        '''H4 { font-size: 10pt; color: black; }
        '''PRE { BACKGROUND-COLOR: #FBEDBB; FONT-FAMILY: &quot;Courier New&quot;, Courier, mono; WHITE-SPACE: pre; }
        '''CODE { COLOR: #990000; FONT-FAMILY: &quot;Courier New&quot;, Courier, mono; }
        '''.S [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property CPOL() As String
            Get
                Return ResourceManager.GetString("CPOL", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to                     GNU GENERAL PUBLIC LICENSE
        '''                       Version 3, 29 June 2007
        '''
        ''' Copyright (C) 2007 Free Software Foundation, Inc. &lt;http://fsf.org/&gt;
        ''' Everyone is permitted to copy and distribute verbatim copies
        ''' of this license document, but changing it is not allowed.
        '''
        '''                            Preamble
        '''
        '''  The GNU General Public License is a free, copyleft license for
        '''software and other kinds of works.
        '''
        '''  The licenses for most software and other practical works are designed
        '''to [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property gpl() As String
            Get
                Return ResourceManager.GetString("gpl", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Byte[].
        '''</summary>
        Friend ReadOnly Property help() As Byte()
            Get
                Dim obj As Object = ResourceManager.GetObject("help", resourceCulture)
                Return CType(obj,Byte())
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to This program is free software: you can redistribute it and/or modify
        '''it under the terms of the GNU General Public License as published by
        '''the Free Software Foundation, either version 3 of the License, or
        '''any later version.
        '''
        '''This program is distributed in the hope that it will be useful,
        '''but WITHOUT ANY WARRANTY; without even the implied warranty of
        '''MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
        '''GNU General Public License for more details.
        '''
        '''You should have received a copy of the GNU  [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property license() As String
            Get
                Return ResourceManager.GetString("license", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Name	MIME Type / Internet Media Type	File Extension	More Details
        '''3D Crossword Plugin	application/vnd.hzn-3d-crossword	.x3d	IANA: 3D Crossword Plugin
        '''3GP	video/3gpp	.3gp	Wikipedia: 3GP
        '''3GP2	video/3gpp2	.3g2	Wikipedia: 3G2
        '''3GPP MSEQ File	application/vnd.mseq	.mseq	IANA: 3GPP MSEQ File
        '''3M Post It Notes	application/vnd.3m.post-it-notes	.pwn	IANA: 3M Post It Notes
        '''3rd Generation Partnership Project - Pic Large	application/vnd.3gpp.pic-bw-large	.plb	3GPP
        '''3rd Generation Partnership Project - Pic Small	appli [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property List_of_MIME_types___Internet_Media_Types_() As String
            Get
                Return ResourceManager.GetString("List_of_MIME_types___Internet_Media_Types_", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
