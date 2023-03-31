using System.Collections;
using System.Collections.Generic;

namespace Extension
{


	public class PriorityQueue<T>
	{

		List<Slot<T>> elements = new List<Slot<T>>();
		


		/// <summary>
		/// Return the total number of elements currently in the Queue.
		/// </summary>
		/// <returns>Total number of elements currently in Queue</returns>
		public int Count
		{
			get { return elements.Count; }
		}


		/// <summary>
		/// Add given item to Queue and assign item the given priority value.
		/// </summary>
		/// <param name="item">Item to be added.</param>
		/// <param name="priorityValue">Item priority value as Double.</param>
		public void Enqueue(T item, double priorityValue)
		{
			Slot<T> elem = new Slot<T>();
			elem.weight = priorityValue;
			int index = 0;
			for (int i = 0; i < Count; i++)
            {
                if (elements[i].weight > priorityValue)
                {
					index = i;
					break;
                }
            }
			elements.Insert(index, elem);

		}


		/// <summary>
		/// Return lowest priority value item and remove item from Queue.
		/// </summary>
		/// <returns>Queue item with lowest priority value.</returns>
		public T Dequeue()
		{
			int bestPriorityIndex = 0;

			for (int i = 0; i < elements.Count; i++)
			{
				if (elements[i].weight < elements[bestPriorityIndex].weight)
				{
					bestPriorityIndex = i;
				}
			}

			T bestItem = elements[bestPriorityIndex].elem;
			elements.RemoveAt(bestPriorityIndex);
			return bestItem;
		}


		/// <summary>
		/// Return lowest priority value item without removing item from Queue.
		/// </summary>
		/// <returns>Queue item with lowest priority value.</returns>
		public T Peek()
		{
			int bestPriorityIndex = 0;

			for (int i = 0; i < elements.Count; i++)
			{
				if (elements[i].weight < elements[bestPriorityIndex].weight)
				{
					bestPriorityIndex = i;
				}
			}

			T bestItem = elements[bestPriorityIndex].elem;
			return bestItem;
		}
	}

	struct Slot<T>
    {
		public T elem;
		public double weight;
    }
}
