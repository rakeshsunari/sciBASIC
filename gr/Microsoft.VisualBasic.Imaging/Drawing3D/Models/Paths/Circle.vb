﻿#Region "Microsoft.VisualBasic::1001e8bd65a42dee0c4c65578f8005f2, gr\Microsoft.VisualBasic.Imaging\Drawing3D\Models\Paths\Circle.vb"

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

    '     Class Circle
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '     Class Arc
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Circle
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Math
Imports Circle2D = Microsoft.VisualBasic.Imaging.Drawing2D.Shapes.Circle

Namespace Drawing3D.Models.Isometric.Paths

    ''' <summary>
    ''' Created by fabianterhorst on 01.04.17.
    ''' </summary>
    Public Class Circle : Inherits Path3D

        <Obsolete>
        Public Sub New(origin As Point3D, radius#)
            Call Me.New(origin, radius, 20)
        End Sub

        ''' <summary>
        ''' 构建三维空间之中的一个圆弧路径
        ''' </summary>
        ''' <param name="origin">相对坐标系原点</param>
        ''' <param name="radius"></param>
        ''' <param name="vertices">构成这个圆形的顶点的数量</param>
        Public Sub New(origin As Point3D, radius#, vertices%)
            Call MyBase.New()

            For Each v As PointF In Circle2D.PathIterator(origin.X, origin.Y, radius, vertices)
                Call Push(v.TupleZ(origin.Z))
            Next
        End Sub
    End Class

    ''' <summary>
    ''' 圆弧
    ''' </summary>
    Public Class Arc : Inherits Path3D

        Public Sub New(origin As Point3D, radius#, startAngle#, sweepAngle#, vertices#)
            Call MyBase.New

            Dim deltaAngle# = 2 * Math.PI * (sweepAngle / 360) / vertices
            Dim angle# = 2 * Math.PI * (startAngle / 360)

            For i As Integer = 0 To vertices - 1
                Dim p As New Point3D(
                    (radius * Cos(angle)) + origin.X,
                    (radius * Sin(angle)) + origin.Y,
                    origin.Z)

                angle += deltaAngle

                Call Push(p)
            Next

            ' push圆心
            Call Push(origin)
        End Sub

        ''' <summary>
        ''' Create a <see cref="Path3D"/> model that similar with <see cref="Paths.Circle"/> model.
        ''' </summary>
        ''' <param name="origin"></param>
        ''' <param name="radius#"></param>
        ''' <param name="vertices#"></param>
        ''' <returns></returns>
        Public Shared Function Circle(origin As Point3D, radius#, vertices#) As Arc
            Return New Arc(origin, radius, 0, 360, vertices)
        End Function
    End Class
End Namespace
