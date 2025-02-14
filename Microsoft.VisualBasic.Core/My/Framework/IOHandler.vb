﻿#Region "Microsoft.VisualBasic::4eca099d62522f9c3be910f7f77f99f3, Microsoft.VisualBasic.Core\My\Framework\IOHandler.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    '     Module IOHandler
    ' 
    ' 
    '         Delegate Function
    ' 
    '             Function: GetWrite, IsRegister, SaveJSON, SaveXml
    ' 
    '             Sub: RegisterHandle
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Text
Imports Microsoft.VisualBasic.Language.Default
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace My.FrameworkInternal

    ''' <summary>
    ''' Collection IO extensions
    ''' </summary>
    ''' <remarks>
    ''' 在这个模块之中删除了read函数，因为read是取决于输入的文件的具体格式的，并不是取决于代码的
    ''' 所以读取操作无意义
    ''' 但是对于write而言，则是可以通过代码来控制具体的输出文件的
    ''' </remarks>
    Public Module IOHandler

        Public Delegate Function ISave(obj As IEnumerable, path As String, encoding As Encoding) As Boolean

        ReadOnly saveWrite As New Dictionary(Of Type, ISave)
        ReadOnly defaultWriter As New [Default](Of ISave)(AddressOf SaveJSON)

        Public Function GetWrite(type As Type) As ISave
            Return saveWrite.TryGetValue(type) Or defaultWriter
        End Function

        Public Function IsRegister(type As Type) As Boolean
            Return saveWrite.ContainsKey(type)
        End Function

        Public Sub RegisterHandle(handle As ISave, type As Type)
            saveWrite(type) = handle
        End Sub

        Public Function SaveJSON(obj As IEnumerable, path As String, encoding As Encoding) As Boolean
            Return GetObjectJson(obj, obj.GetType).SaveTo(path, encoding)
        End Function

        Public Function SaveXml(obj As IEnumerable, path As String, encoding As Encoding) As Boolean
            Return GetXml(obj, obj.GetType).SaveTo(path, encoding)
        End Function
    End Module
End Namespace
