using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Vector_Graphics_App_v2.ShapeClass;

namespace Vector_Graphics_App_v2
{
    internal class MementoClass
    {
        // Originator class
        public class Originator
        {
            public List<Shape> state;

            public void setState(List<Shape> newState)
            {
                state = newState;
            }

            public Memento saveToMemento()
            {
                return new Memento(state);
            }

            public List<Shape> restoreFromMemento(Memento memento)
            {
                state = memento.getSavedList();

                return state;
            }
        }

        // Memento class
        public class Memento
        {
            public List<Shape> state;

            //Setter
            public Memento(List<Shape> savedState)
            {
                state = savedState;
            }

            //Getter
            public List<Shape> getSavedList()
            {
                return state;
            }
        }

        // Caretaker class
        public class Caretaker
        {
            private Stack<Memento> pastMementos = new Stack<Memento>();
            private Stack<Memento> futureMementos = new Stack<Memento>();

            public int getPastCount()
            {
                return pastMementos.Count;
            }

            public int getFutureCount()
            {
                return futureMementos.Count;
            }

            public void addPastMemento(Memento memento)
            {
                pastMementos.Push(memento);
            }

            public void addFutureMemento(Memento memento)
            {
                futureMementos.Push(memento);
            }

            public Memento getPastMemento()
            {
                Memento memento = pastMementos.Pop();
                return memento;
            }

            public Memento getFutureMemento()
            {
                Memento memento = futureMementos.Pop();
                return memento;
            }

            public void clearPastMementos()
            {
                pastMementos = new Stack<Memento>();
            }

            public void clearFutureMementos()
            {
                futureMementos = new Stack<Memento>();
            }
        }
    }
}
