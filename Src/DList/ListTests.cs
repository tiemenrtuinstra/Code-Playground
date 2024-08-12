using System;
using System.Linq;
using Xunit;

namespace DList.Tests;

public class ListTests
{
    private static Random random = new Random();

    [Fact]
    public void TestList_New_CreatesEmptyList()
    {
        var list = new List<int>();
        Assert.Equal(0, list.Size());
    }

    [Fact]
    public void TestList_PushFront_AddsValueToTheList()
    {
        var list = new List<int>();
        list.PushFront(10);

        Assert.Equal(1, list.Size());
    }

    [Fact]
    public void TestList_PushFront_AddsValuesToTheFront()
    {
        var list = new List<int>();
        list.PushFront(10);
        list.PushFront(20);

        Assert.Equal(2, list.Size());
        Assert.Equal(20, list.ValueAt(0));
        Assert.Equal(10, list.ValueAt(1));
    }

    [Fact]
    public void TestList_PushAllFront_AddsAllValuesToTheFront()
    {
        var vals = RandomValues(10);
        var list = new List<int>();
        list.PushAllFront(vals);

        Assert.Equal(vals.Length, list.Size());

        for (int i = 0; i < vals.Length; i++)
        {
            var exp = vals[vals.Length - i - 1];
            var act = list.ValueAt(i);
            Assert.Equal(exp, act);
        }
    }

    [Fact]
    public void TestList_PushBack_AddsValueToTheList()
    {
        var list = new List<int>();
        list.PushBack(10);

        Assert.Equal(1, list.Size());
    }

    [Fact]
    public void TestList_PushBack_AddsValuesToTheBack()
    {
        var list = new List<int>();
        list.PushBack(10);
        list.PushBack(20);

        Assert.Equal(2, list.Size());
        Assert.Equal(10, list.ValueAt(0));
        Assert.Equal(20, list.ValueAt(1));
    }

    [Fact]
    public void TestList_PushAllBack_AddsAllValuesToTheBack()
    {
        var vals = RandomValues(10);
        var list = new List<int>();
        list.PushAllBack(vals);

        Assert.Equal(vals.Length, list.Size());

        for (int i = 0; i < vals.Length; i++)
        {
            var act = list.ValueAt(i);
            Assert.Equal(vals[i], act);
        }
    }

    [Fact]
    public void TestList_PopFront_OnSingleElementListBecomesEmpty()
    {
        var list = new List<int>();
        list.PushFront(10);
        list.PopFront();

        Assert.Equal(0, list.Size());
    }

    [Fact]
    public void TestList_PopFront_RemovesValuesFromTheFront()
    {
        var vals = RandomValues(10);
        var list = new List<int>();
        list.PushAllBack(vals);

        foreach (var v in vals)
        {
            var (act, ok) = list.PopFront();
            Assert.True(ok);
            Assert.Equal(v, act);
        }

        Assert.Equal(0, list.Size());
    }

    [Fact]
    public void TestList_PopBack_OnSingleElementListBecomesEmpty()
    {
        var list = new List<int>();
        list.PushBack(10);
        list.PopBack();

        Assert.Equal(0, list.Size());
    }

    [Fact]
    public void TestList_PopBack_RemovesValuesFromTheBack()
    {
        var vals = RandomValues(10);
        var list = new List<int>();
        list.PushAllFront(vals);

        foreach (var v in vals)
        {
            var (act, ok) = list.PopBack();
            Assert.True(ok);
            Assert.Equal(v, act);
        }

        Assert.Equal(0, list.Size());
    }

    [Fact]
    public void TestList_ValueAt()
    {
        var vals = RandomValues(10);
        var list = new List<int>();
        list.PushAllBack(vals);

        for (int i = 0; i < vals.Length; i++)
        {
            var act = list.ValueAt(i);
            Assert.Equal(vals[i], act);
        }
    }

    [Fact]
    public void TestList_InsertAt_InsertingAtTheFront()
    {
        var vals = RandomValues(3);
        var list = new List<int>();
        list.PushAllBack(vals);

        list.InsertAt(0, 9999);

        Assert.Equal(9999, list.ValueAt(0));

        for (int i = 0; i < vals.Length; i++)
        {
            var act = list.ValueAt(i + 1);
            Assert.Equal(vals[i], act);
        }
    }

    [Fact]
    public void TestList_InsertAt_InsertingInTheMiddle()
    {
        var list = new List<int>();
        list.PushAllBack(1, 2, 3, 4, 5);

        list.InsertAt(2, 9);

        var iter = list.Iterator();

        foreach (var v in new[] { 1, 2, 9, 3, 4, 5 })
        {
            var act = iter.Next();
            Assert.Equal(v, act);
        }
    }

    [Fact]
    public void TestList_Iterator()
    {
        var vals = RandomValues(10);
        var list = new List<int>();
        list.PushAllBack(vals);

        var iter = list.Iterator();
        int i = 0;

        while (iter.HasNext())
        {
            var exp = vals[i];
            var act = iter.Next();
            i++;
            Assert.Equal(exp, act);
        }

        Assert.Equal(vals.Length, list.Size());
    }

    [Fact]
    public void TestList_ReverseIterator()
    {
        var vals = RandomValues(10);
        var list = new List<int>();
        list.PushAllFront(vals);

        var iter = list.ReverseIterator();
        int i = 0;

        while (iter.HasNext())
        {
            var exp = vals[i];
            var act = iter.Next();
            i++;
            Assert.Equal(exp, act);
        }

        Assert.Equal(vals.Length, list.Size());
    }

    private static int[] RandomValues(int size)
    {
        return Enumerable.Range(0, size).Select(_ => random.Next(100)).ToArray();
    }
}
