﻿#Region "Microsoft.VisualBasic::5358ad4da98838b9dabf00acc48c73fe, Microsoft.VisualBasic.Core\ComponentModel\Ranges\RangeModel\Range.vb"

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

    '     Class Range
    ' 
    '         Properties: Max, Min
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: (+2 Overloads) IsInside, IsOverlapping, ToString
    ' 
    '     Class RangeTagValue
    ' 
    '         Properties: Value
    ' 
    '         Constructor: (+2 Overloads) Sub New
    '         Function: ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Serialization.JSON

Namespace ComponentModel.Ranges.Model

    Public Class Range(Of T As IComparable)
        Implements IRanges(Of T)

        Public ReadOnly Property Min As T Implements IRanges(Of T).Min
        Public ReadOnly Property Max As T Implements IRanges(Of T).Max

        Sub New(min As T, max As T)
            Me.Min = min
            Me.Max = max
        End Sub

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function

        Public Function IsInside(x As T) As Boolean Implements IRanges(Of T).IsInside
            Return (Language.GreaterThanOrEquals(x, Min) AndAlso Language.LessThanOrEquals(x, Max))
        End Function

        Public Function IsInside(range As IRanges(Of T)) As Boolean Implements IRanges(Of T).IsInside
            Return ((IsInside(range.Min)) AndAlso (IsInside(range.Max)))
        End Function

        Public Function IsOverlapping(range As IRanges(Of T)) As Boolean Implements IRanges(Of T).IsOverlapping
            Return ((IsInside(range.Min)) OrElse (IsInside(range.Max)))
        End Function
    End Class

    Public Class RangeTagValue(Of T As IComparable, V) : Inherits Range(Of T)

        Public Property Value As V

        Sub New(min As T, max As T)
            Call MyBase.New(min, max)
        End Sub

        Sub New(min As T, max As T, value As V)
            MyBase.New(min, max)
            Me.Value = value
        End Sub

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class
End Namespace
