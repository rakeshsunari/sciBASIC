﻿#Region "Microsoft.VisualBasic::217afa2cb9160b3e9fb40fd75f7f495b, Microsoft.VisualBasic.Core\ComponentModel\DataSource\Repository\ILocalSearchHandle.vb"

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

    '     Interface ILocalSearchHandle
    ' 
    '         Function: Match, Matches
    ' 
    '     Module SearchFramework
    ' 
    '         Function: MultipleQuery, Query
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices

Namespace ComponentModel.DataSourceModel.Repository

    Public Interface ILocalSearchHandle

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Keyword"></param>
        ''' <param name="CaseSensitive">是否大小写敏感，默认不敏感</param>
        ''' <returns></returns>
        Function Matches(Keyword As String, Optional CaseSensitive As CompareMethod = CompareMethod.Text) As ILocalSearchHandle()
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Keyword"></param>
        ''' <param name="CaseSensitive">是否大小写敏感，默认不敏感</param>
        ''' <returns></returns>
        Function Match(Keyword As String, Optional CaseSensitive As CompareMethod = CompareMethod.Text) As Boolean
    End Interface

    ''' <summary>
    ''' Extensions for data query
    ''' </summary>
    Public Module SearchFramework

        <Extension>
        Public Iterator Function MultipleQuery(Of T, Term)(source As IEnumerable(Of T),
                                                           query As IEnumerable(Of NamedValue(Of Term())),
                                                           assert As Func(Of T, Term, Boolean)) As IEnumerable(Of NamedValue(Of Map(Of Term, T)))

            Dim terms As NamedValue(Of Term())() = query.ToArray

            For Each entity As T In source
                For Each block In query
                    For Each match As Term In block.Value
                        If assert(entity, match) Then
                            Yield New NamedValue(Of Map(Of Term, T)) With {
                                .Name = block.Name,
                                .Value = New Map(Of Term, T) With {
                                    .Key = match,
                                    .Maps = entity
                                }
                            }
                        End If
                    Next
                Next
            Next
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function Query(Of T, Term)(source As IEnumerable(Of T), queries As IEnumerable(Of Term), assert As Func(Of T, Term, Boolean)) As IEnumerable(Of Map(Of Term, T))
            Return source.MultipleQuery({New NamedValue(Of Term())("null", queries.ToArray)}, assert).Values
        End Function
    End Module
End Namespace
