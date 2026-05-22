
// * Pizzería Campus Express - Gestión de pedidos con Queue y Stack
// * Compatible con SharpDevelop 4.4 / .NET Framework 2.0+
	 

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace laboratoriPizzeriaCampusExpress
{
    public partial class MainForm : Form
    {
        // Colecciones principales: FIFO para pedidos, LIFO para bitácora
        private Queue<string> colaPedidos = new Queue<string>();
        private Stack<string> pilaBitacora = new Stack<string>();
        private Queue<string> pedidoPremium = new Queue<string>();

        public MainForm()
        {
            InitializeComponent();
            ActualizarUI();
        }

        // PASO 1: Nuevo pedido (FIFO entrada)
        private void BtnNuevoPedido_Click(object sender, EventArgs e)
        {
            string cliente = txtCliente.Text.Trim();

            // Validar entrada
            if (cliente == "")
            {
                lblEstado.Text = string.Format("⚠️ Debe ingresar un nombre de cliente.");
                return;
            }

            // Agregar a la cola
            colaPedidos.Enqueue(cliente);

            // Registrar en la pila
            pilaBitacora.Push(string.Format("PEDIDO: {0}", cliente));

            // Limpiar campo y actualizar
            txtCliente.Clear();
            lblEstado.Text = string.Format("✅ Pedido registrado para {0}", cliente);
            ActualizarUI();
        }

        // PASO 2: Entregar pedido (FIFO salida)
        private void BtnEntregar_Click(object sender, EventArgs e)
        {
            atenderSiguiente();
        }

        // PASO 3: Deshacer última acción (LIFO + lógica de reversión)
        private void BtnDeshacer_Click(object sender, EventArgs e)
        {
            if (pilaBitacora.Count == 0)
            {
                lblEstado.Text = string.Format("📭 No hay acciones para deshacer.");
                return;
            }

            string ultimaAccion = pilaBitacora.Pop();

            if (ultimaAccion.StartsWith("PEDIDO:"))
            {
                // Extraer nombre del cliente
                string nombre = ultimaAccion.Replace("PEDIDO: ", "").Trim();
                // Reconstruir cola excluyendo ese pedido
                string[] temporal = colaPedidos.ToArray();
                colaPedidos.Clear();
                foreach (string p in temporal)
                {
                    if (p != nombre)
                        colaPedidos.Enqueue(p);
                }
                lblEstado.Text = string.Format("↩️ Se deshizo el pedido de {0}", nombre);
            }
            else if (ultimaAccion.StartsWith("ENTREGADO:"))
            {
                // Extraer nombre del cliente
                string nombre = ultimaAccion.Replace("ENTREGADO: ", "").Trim();
                // Volver a encolar
                colaPedidos.Enqueue(nombre);
                lblEstado.Text = string.Format("↩️ Se deshizo la entrega a {0}", nombre);
            }
            else
            {
                lblEstado.Text = string.Format("⚠️ Acción desconocida en bitácora.");
            }

            ActualizarUI();
            atenderSiguiente();
        }

        // PASO 4: Limpiar todo (reiniciar sistema)
        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            colaPedidos.Clear();
            pilaBitacora.Clear();
            lblEstado.Text = string.Format("🧹 Sistema reiniciado.");
            ActualizarUI();
            atenderSiguiente();
        }

        // Sincronizar la interfaz con el estado actual
        private void ActualizarUI()
        {
            // Limpiar listas visuales
            lstPedidos.Items.Clear();
            lstBitacora.Items.Clear();

            // Mostrar cola de pedidos
            foreach (string p in colaPedidos)
                lstPedidos.Items.Add(p);
            if (colaPedidos.Count == 0)
                lstPedidos.Items.Add("(Sin pedidos pendientes)");

            // Mostrar bitácora (pila)
            foreach (string accion in pilaBitacora)
                lstBitacora.Items.Add(accion);
            if (pilaBitacora.Count == 0)
                lstBitacora.Items.Add("(Sin acciones registradas)");

            // Actualizar contador
            lblContador.Text = string.Format("Pedidos: {0} | Bitácora: {1}",
                colaPedidos.Count, pilaBitacora.Count);
        }
        private void atenderSiguiente()
		{
		    string CLIENTE = "";
		    string tipodesuPedido =  "";
		    
		    if (pedidoPremium.Count > 0)
		    {
		       CLIENTE = pedidoPremium.Dequeue();
		        tipodesuPedido = "premiun";
		    }

		    else if (colaPedidos.Count > 0)
		    {
		        CLIENTE = colaPedidos.Dequeue();
		        tipodesuPedido = "normal";
		    }
		    else
		    {
		        lblEstado.Text = "📭 NO existen pedidos en ninguna de las colas.";
		        return;
		    }
		    
			string mensajeBitacora = string.Format("ENTREGADO ({0}): {1}", tipodesuPedido, CLIENTE);
		    pilaBitacora.Push(mensajeBitacora);
		    lblEstado.Text = string.Format("🍕 Pedido {0} entregado a {1}",tipodesuPedido, CLIENTE);
		    ActualizarUI();
		}
        
       
        void BtnPedidoPremiunClick(object sender, EventArgs e)
        {
        	string cliente = txtCliente.Text.Trim();
        	if (cliente == "")
            {
	            lblEstado.Text = string.Format("⚠️ Debe ingresar un nombre de cliente.");
	            return;
            }
		
		    pedidoPremium.Enqueue(cliente);
		    pilaBitacora.Push(string.Format("pedido premium: {0}", cliente));
		    
		    txtCliente.Clear();
		    lblEstado.Text = string.Format("⭐ Pedido premin listo para {0}", cliente);
		    ActualizarUI();
        	
        }
    }
}