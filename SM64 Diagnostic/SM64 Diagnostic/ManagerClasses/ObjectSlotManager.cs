using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;
using System.Drawing;

namespace SM64_Diagnostic.ManagerClasses
{
    public class ObjectSlotManager
    {
        public ObjectSlot[] ObjectSlots;
        public ObjectSlotData[] ObjectSlotData;

        Config _config;
        ObjectAssociations _objectAssoc;
        ObjectManager _objManager;
        ProcessStream _stream;

        Dictionary<uint, int> _memoryAddressSlotIndex;
        int _selectedSlot;

        public PictureBox Trash;
        public uint? SelectedAddress = null;
        public const byte VacantGroup = 0xFF;

        public enum SortMethodType {ProcessingOrder, MemoryOrder, LinkListOrder };

        public SortMethodType SortMethod = SortMethodType.ProcessingOrder;

        public int MaxSlots
        {
            get
            {
                return _config.ObjectSlots.MaxSlots;
            }
        }

        public int SelectedSlot
        {
            get
            {
                return _selectedSlot;
            }
            set
            {
                SelectedAddress = ObjectSlotData.First((objData) => objData.Index == value).Address;
                _objManager.CurrentAddress = SelectedAddress.Value;
                _selectedSlot = value;
            }
        }

        public ObjectSlotManager(ProcessStream stream, Config config, ObjectAssociations objAssoc, ObjectManager objManager, PictureBox trash)
        {
            Trash = trash;
            Trash.AllowDrop = true;
            Trash.DragEnter += OnTrashDragOver;
            Trash.DragDrop += OnTrashDrop;

            _config = config;
            _objectAssoc = objAssoc;
            _stream = stream;
            _stream.OnUpdate += OnUpdate;
            _objManager = objManager;

            // Create and setup object slots
            ObjectSlots = new ObjectSlot[_config.ObjectSlots.MaxSlots];
            ObjectSlotData = new ObjectSlotData[_config.ObjectSlots.MaxSlots];
            for (int i = 0; i < _config.ObjectSlots.MaxSlots; i++)
            {
                var objectSlot = new ObjectSlot(i, this);
                ObjectSlots[i] = objectSlot;
                objectSlot.Image = _objectAssoc.EmptyImage;
            }
        }

        public void AddToControls(Control.ControlCollection controls)
        {
            foreach (ObjectSlot obj in ObjectSlots)
            {
                controls.Add(obj.Control);
            }
        }

        public void OnTrashDragOver(object sender, DragEventArgs e)
        {
            // Make sure we have valid Drag and Drop data (it is an index)
            if (!e.Data.GetDataPresent(typeof(DropAction)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            var dropAction = ((DropAction)e.Data.GetData(typeof(DropAction))).Action;
            if (dropAction != DropAction.ActionType.Object)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;
        }

        public void SwapSlots(int index1, int index2)
        {
            MessageBox.Show("Currently Unimplemented!");
            return;
        }

        public void OnTrashDrop(object sender, DragEventArgs e)
        {
            // Make sure we have valid Drag and Drop data (it is an index)
            if (!e.Data.GetDataPresent(typeof(DropAction)))
                return;

            var dropAction = ((DropAction)e.Data.GetData(typeof(DropAction)));
            if (dropAction.Action != DropAction.ActionType.Object)
                return;

            UnloadSlot(dropAction.Address);
        }

        private void UnloadSlot(uint address)
        {
            _stream.WriteRam(new byte[] { 0x00, 0x00 }, address + _config.ObjectSlots.ObjectActiveOffset);
        }

        private ObjectSlotData[] GetProcessedObjects(ObjectGroupsConfig groupConfig, ObjectSlotsConfig slotConfig)
        {
            var newObjectSlotData = new ObjectSlotData[slotConfig.MaxSlots];

            // Loop through each processing group
            int currentSlot = 0;
            foreach (var objectProcessGroup in groupConfig.ProcessingGroups)
            {
                uint processGroupStructAddress = groupConfig.FirstGroupingAddress + objectProcessGroup * groupConfig.ProcessGroupStructSize;

                // Calculate start and ending objects
                uint currentGroupObject = BitConverter.ToUInt32(_stream.ReadRam(processGroupStructAddress
                    + groupConfig.ProcessNextLinkOffset, 4), 0);

                // Make sure there are objects within the group
                if (currentGroupObject == processGroupStructAddress)
                    continue;


                // Loop through every object within the group
                while ((currentGroupObject != processGroupStructAddress && currentSlot < slotConfig.MaxSlots))
                {

                    // Validate current object
                    if (BitConverter.ToUInt16(_stream.ReadRam(currentGroupObject + _config.ObjectSlots.HeaderOffset, 2), 0) != 0x18)
                        return null;

                    // Get data
                    newObjectSlotData[currentSlot].Address =currentGroupObject;
                    newObjectSlotData[currentSlot].ObjectProcessGroup = objectProcessGroup;
                    newObjectSlotData[currentSlot].Index = currentSlot;
                    newObjectSlotData[currentSlot].VacantSlotIndex = null;

                    // Move to next object
                    currentGroupObject = BitConverter.ToUInt32(
                    _stream.ReadRam(currentGroupObject + groupConfig.ProcessNextLinkOffset, 4), 0);

                    // Mark next slot
                    currentSlot++;
                }
            }

            var vacantSlotStart = currentSlot;

            // Now calculate vacant addresses
            uint currentObject = BitConverter.ToUInt32(_stream.ReadRam(groupConfig.VactantPointerAddress, 4), 0);
            for (; currentSlot < slotConfig.MaxSlots; currentSlot++)
            {
                // Validate current object
                if (BitConverter.ToUInt16(_stream.ReadRam(currentObject + _config.ObjectSlots.HeaderOffset, 2), 0) != 0x18)
                    return null;

                newObjectSlotData[currentSlot].Address = currentObject;
                newObjectSlotData[currentSlot].ObjectProcessGroup = VacantGroup;
                newObjectSlotData[currentSlot].Index = currentSlot;
                newObjectSlotData[currentSlot].VacantSlotIndex = currentSlot - vacantSlotStart;

                currentObject = BitConverter.ToUInt32(
                    _stream.ReadRam(currentObject + groupConfig.ProcessNextLinkOffset, 4), 0);
            }

            return newObjectSlotData;
        }

        public void OnUpdate(object sender, EventArgs e)
        {
            var groupConfig = _config.ObjectGroups;
            var slotConfig = _config.ObjectSlots;

            var newObjectSlotData = GetProcessedObjects(groupConfig, slotConfig);

            if (newObjectSlotData == null)
                return;

            // Processing sort order
            switch (SortMethod)
            {
                case SortMethodType.ProcessingOrder:
                    // Data is already sorted
                    break;

                case SortMethodType.MemoryOrder:
                    // Create Memory-SlotIndex lookup table
                    if (_memoryAddressSlotIndex == null)
                    {
                        _memoryAddressSlotIndex = new Dictionary<uint, int>();
                        var sortedList = newObjectSlotData.OrderBy((objData) => objData.Address).ToList();
                        foreach (var objectData in newObjectSlotData)
                        {
                            _memoryAddressSlotIndex.Add(objectData.Address,
                                sortedList.FindIndex((objData) => objectData.Address == objData.Address));
                        }
                    }
                    // Order by address
                    for (int i = 0; i < slotConfig.MaxSlots; i++)
                    {
                        newObjectSlotData[i].Index = _memoryAddressSlotIndex[newObjectSlotData[i].Address];
                    }
                    break;

                case SortMethodType.LinkListOrder:
                    SortMethod = SortMethodType.ProcessingOrder;
                    MessageBox.Show("Currently Unimplemented!");
                    break;
            }

            // Update slots
            foreach (var objectData in newObjectSlotData)
            {
                uint currentAddress = objectData.Address;
                int index = objectData.Index;
                var isActive = BitConverter.ToUInt16(_stream.ReadRam(currentAddress + _config.ObjectSlots.ObjectActiveOffset, 2), 0) != 0x0000;

                var behaviorScriptAdd = BitConverter.ToUInt32(_stream.ReadRam(currentAddress + _config.ObjectSlots.BehaviorScriptOffset, 4), 0)
                    & 0x0FFFFFFF;

                var newImage = _objectAssoc.GetObjectImage(behaviorScriptAdd, !isActive);
                ObjectSlots[index].Image = newImage;

                var newColor = objectData.ObjectProcessGroup == VacantGroup ? groupConfig.VacantSlotColor :
                    groupConfig.ProcessingGroupsColor[objectData.ObjectProcessGroup];
                ObjectSlots[index].BackColor = newColor;

                ObjectSlots[index].Text = (SortMethod == SortMethodType.ProcessingOrder && objectData.VacantSlotIndex.HasValue) ?
                    String.Format("VS{1}", objectData.Index, objectData.VacantSlotIndex.Value) : objectData.Index.ToString();

                // Update object manager image
                if (SelectedAddress.HasValue && SelectedAddress.Value == objectData.Address)
                {
                    _objManager.BackColor = newColor;
                    _objManager.Image = newImage;
                    _objManager.Update();   
                }
            }
            ObjectSlotData = newObjectSlotData;
        }

        public void OnSlotDropAction(DropAction dropAction, ObjectSlot objSlot)
        {
            switch (dropAction.Action)
            {
                case DropAction.ActionType.Mario:
                    // Move mario to object
                    var marioAddress = _config.Mario.MarioPointerAddress;
                    var objectAddress = ObjectSlotData.First((objData) => objData.Index == objSlot.Index).Address;

                    // Get object position
                    float x, y, z;
                    x = BitConverter.ToSingle(_stream.ReadRam(objectAddress + _config.ObjectSlots.ObjectXOffset, 4), 0);
                    y = BitConverter.ToSingle(_stream.ReadRam(objectAddress + _config.ObjectSlots.ObjectYOffset, 4), 0);
                    z = BitConverter.ToSingle(_stream.ReadRam(objectAddress + _config.ObjectSlots.ObjectZOffset, 4), 0);

                    // Add offset
                    y += 300f;

                    // Move mario to object
                    _stream.WriteRam(BitConverter.GetBytes(x), marioAddress + _config.Mario.XOffset);
                    _stream.WriteRam(BitConverter.GetBytes(y), marioAddress + _config.Mario.YOffset);
                    _stream.WriteRam(BitConverter.GetBytes(z), marioAddress + _config.Mario.ZOffset);
                    break;

                case DropAction.ActionType.Object:
                    break;

                default:
                    return;
            }
        }
    }
}