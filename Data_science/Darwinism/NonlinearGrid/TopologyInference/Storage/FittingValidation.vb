﻿#Region "Microsoft.VisualBasic::1c61f318830aae673c1faa473bdbe22c, Data_science\Darwinism\NonlinearGrid\TopologyInference\Storage\FittingValidation.vb"

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

    ' Class FittingValidation
    ' 
    '     Properties: actual, errors, predicts, sampleID
    ' 
    '     Function: ToString
    ' 
    ' /********************************************************************************/

#End Region

Public Class FittingValidation

    Public Property sampleID As String
    Public Property actual As Double
    Public Property predicts As Double

    Public ReadOnly Property errors As Double
        Get
            Return Math.Abs(actual - predicts)
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return $"[{errors}] {sampleID}"
    End Function
End Class
