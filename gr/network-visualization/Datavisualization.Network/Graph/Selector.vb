﻿#Region "Microsoft.VisualBasic::17453a7bb03cd53c554513941562f463, gr\network-visualization\Datavisualization.Network\Graph\Selector.vb"

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

    '     Module Selector
    ' 
    '         Function: GenericSelector, SelectEdgeValue, SelectNodeValue
    '         Interface IGraphValueContainer
    ' 
    '             Properties: data
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.DataFramework

Namespace Graph

    ''' <summary>
    ''' Graph value selector by property name
    ''' </summary>
    Public Module Selector

        ''' <summary>
        ''' An class object that contains <see cref="GraphData"/>
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        Public Interface IGraphValueContainer(Of T As GraphData)
            Property data As T
        End Interface

        ''' <summary>
        ''' Create a node value selector from a property name
        ''' </summary>
        ''' <param name="property$"></param>
        ''' <returns></returns>
        <Extension> Public Function SelectNodeValue(property$, Optional ByRef type As Type = Nothing) As Func(Of Node, Object)
            Return [property].GenericSelector(Of NodeData, Node)(type)
        End Function

        ''' <summary>
        ''' 所映射的属性的类型
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <typeparam name="Graph"></typeparam>
        ''' <param name="property$"></param>
        ''' <param name="type"></param>
        ''' <returns></returns>
        <Extension>
        Public Function GenericSelector(Of T As GraphData, Graph As IGraphValueContainer(Of T))(property$, ByRef type As Type) As Func(Of Graph, Object)
            Dim graphValue = GetType(Graph).Schema(PropertyAccess.Readable,, True)
            Dim reader As PropertyInfo
            Dim dataValues = GetType(T).Schema(PropertyAccess.Readable,, True)

            If graphValue.ContainsKey([property]) Then
                reader = graphValue([property])
                type = reader.PropertyType
                Return Function(model) reader.GetValue(model)
            ElseIf dataValues.ContainsKey([property]) Then
                reader = dataValues([property])
                type = reader.PropertyType
                Return Function(model) reader.GetValue(model.data)
            Else
                type = GetType(String)
                Return Function(model) model.data([property])
            End If
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function SelectEdgeValue(property$, Optional ByRef type As Type = Nothing) As Func(Of Edge, Object)
            Return [property].GenericSelector(Of EdgeData, Edge)(type)
        End Function
    End Module
End Namespace
