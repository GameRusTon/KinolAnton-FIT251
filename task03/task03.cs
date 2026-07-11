using System.Collections;

namespace task03;

public class CustomCollection<T> : IEnumerable<T>
{
    private readonly List<T> _items = new();

    public void Add(T item)
    {
        if (item == null)
        throw new ArgumentNullException(nameof(item), "Добавляемый элемент не может быть null.");

        _items.Add(item);
    }
    public void Remove(T item)
    {
        if (item == null)
        throw new ArgumentNullException(nameof(item), "Удаляемый элемент не может быть null.");
        
        _items.Remove(item);
    }
    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<T> GetReverseEnumerator()
    {
        for (int i = _items.Count - 1; i >= 0; i--)
        {
            yield return _items[i];
        }
    }

    public static IEnumerable <T> GenerateSequence(int start, int count)
    {
        if (count < 0)
        throw new ArgumentOutOfRangeException(nameof(count), "Количество элементов count не может быть меньше нуля.");

        for (int i = 0; i < count; i++)
        {
            int number = start + i;
            yield return (T)(object)number;
        }
    }

    public IEnumerable<T> FilterAndSort(Func<T, bool> predicate, Func<T, IComparable> keySelector)
    {
        if (predicate == null)
        throw new ArgumentNullException(nameof(predicate), "Фильтр predicate не может быть null.");

        if (keySelector == null)
        throw new ArgumentNullException(nameof(keySelector), "Критерий сортировки keySelector не может быть null.");

        return _items.Where(predicate).OrderBy(keySelector);
    }
}
