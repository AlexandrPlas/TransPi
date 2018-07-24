using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TransPi
{
    public class  UndoRedo
    {
        private Stack<PictureMass> _undo = new Stack<PictureMass>(0);
        private Stack<PictureMass> _redo = new Stack<PictureMass>(0);
        private bool _ud, _re;



        public UndoRedo()
        {
            _undo.Clear();
            _redo.Clear();
            _ud = false;
            _re = false;
        }

        public void NextDo(PictureMass Do)
        {
            this._undo.Push(Do);
            this._redo.Clear();
            this._ud = true;
            this._re = false;
        }

        public PictureMass Undo(PictureMass Main)
        {
            PictureMass tmp = new PictureMass(Main);
            PictureMass res = this._undo.Pop();
            this._redo.Push(tmp);
            this._re = true;
            if (this._undo.Count() == 0)
                this._ud = false;
            return res;
        }

        public PictureMass Redo(PictureMass Main)
        {
            PictureMass tmp = new PictureMass(Main);
            PictureMass res = this._redo.Pop();
            this._undo.Push(tmp);
            this._ud = true;
            if (this._redo.Count() == 0)
                this._re = false;
            return res;
        }

        public void ClearUR()
        {
            this._undo.Clear();
            this._redo.Clear();
            this._ud = false;
            this._re = false;
        }

        public bool GetUndo()
        {
            return this._ud;
        }

        public bool GetRedo()
        {
            return this._re;
        }
    }
}
