#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Windows;
using System.Windows.Controls;
using MediaPortal.GUI.Library;

namespace MediaPortal.Drawing.Layouts
{
  public class StackLayout : ILayout
  {
    #region Constructors

    public StackLayout() {}

    public StackLayout(int spacing)
    {
      _spacing = new Size(Math.Max(0, spacing), Math.Max(0, spacing));
    }

    public StackLayout(int spacing, Orientation orientation)
    {
      _spacing = new Size(Math.Max(0, spacing), Math.Max(0, spacing));
      _orientation = orientation;
    }

    public StackLayout(int spacing, Orientation orientation, bool collapseHiddenButtons)
      : this(spacing, orientation)
    {
      _collapseHiddenButtons = collapseHiddenButtons;
    }

    #endregion Constructors

    #region Methods

    private void ApplyAlignment(GUIControl element, Thickness t, double x, double y, double w, double h)
    {
      Rect rect = new Rect(x, y, element.Width, element.Height);

      switch (element.HorizontalAlignment)
      {
        case HorizontalAlignment.Center:
          rect.X = x + ((w - element.Width) / 2);
          break;
        case HorizontalAlignment.Right:
          rect.X = x + w - element.Width;
          break;
        case HorizontalAlignment.Stretch:
          rect.Width = w;
          break;
      }

      switch (element.VerticalAlignment)
      {
        case VerticalAlignment.Center:
          rect.Y = y + ((h - element.Height) / 2);
          break;
        case VerticalAlignment.Bottom:
          rect.Y = y + h - element.Height;
          break;
        case VerticalAlignment.Stretch:
          rect.Height = h;
          break;
      }

      element.Arrange(rect);
    }

    public void Arrange(GUIGroup element)
    {
      Thickness t = element.Margin;
      Point l = element.Location;

      double x = element.Location.X + t.Left;
      double y = element.Location.Y + t.Top;
      double w = _orientation != Orientation.Horizontal ? Math.Max(0, element.Width - t.Width) : 0;
      double h = _orientation == Orientation.Horizontal ? Math.Max(0, element.Height - t.Height) : 0;

      foreach (var child in element.Children)
      {
        if (child.Visibility == Visibility.Collapsed)
        {
          continue;
        }

        if (_orientation == Orientation.Horizontal)
        {
          ApplyAlignment(child, t, x, y, w = child.Width, h);

          x += w + _spacing.Width;

          continue;
        }

        ApplyAlignment(child, t, x, y, w, h = child.Height);

        y += h + _spacing.Height;
      }
    }

    public Size Measure(GUIGroup element, Size availableSize)
    {
      double w = 0;
      double h = 0;

      foreach (var child in element.Children)
      {
        if (child.Visibility == Visibility.Collapsed)
        {
          continue;
        }

        child.Measure(availableSize);

        w = _orientation != Orientation.Horizontal ? Math.Max(w, child.Width) : w + child.Width + _spacing.Width;
        h = _orientation == Orientation.Horizontal ? Math.Max(h, child.Height) : h + child.Height + _spacing.Height;
      }

      Thickness t = element.Margin;

      _size.Width = w + t.Width;
      _size.Height = h + t.Height;

      return _size;
    }

    #endregion Methods

    #region Properties

    public Orientation Orientation
    {
      get { return _orientation; }
      set { _orientation = value; }
    }

    public Size Size
    {
      get { return _size; }
    }

    public Size Spacing
    {
      get { return _spacing; }
      set { _spacing = value; }
    }

    public bool CollapseHiddenButtons
    {
      get { return _collapseHiddenButtons; }
      set { _collapseHiddenButtons = value; }
    }

    #endregion Properties

    #region Fields

    private Orientation _orientation = Orientation.Vertical;
    private Size _spacing = Size.Empty;
    private Size _size = Size.Empty;
    private bool _collapseHiddenButtons = false;

    #endregion Fields
  }
}